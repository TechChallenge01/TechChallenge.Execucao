using Application.Gateways.Execucao;
using Application.Interfaces;
using Application.UseCases.Execucao;
using FluentAssertions;
using Moq;
using Shared.DTOs.Execucao.Input;

namespace UnitTests.Application.UseCases;

public class IniciarPreparacaoUseCaseTests
{
    private readonly Mock<IExecucaoPedidoDataSource> _mockDS = new();
    private readonly Mock<IEventPublisher> _mockPublisher = new();

    private IniciarPreparacaoUseCase CriarUseCase()
        => IniciarPreparacaoUseCase.Create(ExecucaoPedidoGateway.Create(_mockDS.Object), _mockPublisher.Object);

    private static ExecucaoPedidoInputDTO ExecucaoDTO(Guid? pedidoId = null) => new()
    {
        Id = Guid.NewGuid(), PedidoId = pedidoId ?? Guid.NewGuid(), Status = "Recebida",
        Etapas = new() { new() { Status = "Recebida", DataAlteracao = DateTime.UtcNow } },
        IdUsuarioCriacao = Guid.NewGuid(), DataCriacao = DateTime.UtcNow, Ativo = true
    };

    [Fact]
    public async Task Run_QuandoExecucaoExiste_DeveIniciarPreparacaoEPublicarEvento()
    {
        var dto = ExecucaoDTO();
        _mockDS.Setup(x => x.GetByPedidoId(dto.PedidoId, It.IsAny<CancellationToken>())).ReturnsAsync(dto);
        _mockDS.Setup(x => x.Update(It.IsAny<ExecucaoPedidoInputDTO>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockPublisher.Setup(x => x.PublishAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await CriarUseCase().Run(dto.PedidoId, Guid.NewGuid(), CancellationToken.None);

        _mockPublisher.Verify(x => x.PublishAsync("execucao.pedido.em-preparacao", It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockDS.Verify(x => x.Update(It.Is<ExecucaoPedidoInputDTO>(d => d.Status == "EmPreparacao"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Run_QuandoNaoExiste_NaoDeveFazerNada()
    {
        _mockDS.Setup(x => x.GetByPedidoId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExecucaoPedidoInputDTO?)null);

        await CriarUseCase().Run(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);

        _mockPublisher.Verify(x => x.PublishAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
