using Application.Gateways.Execucao;
using Application.Interfaces;
using Application.UseCases.Execucao;
using FluentAssertions;
using Moq;
using Shared.DTOs.Execucao.Input;

namespace UnitTests.Application.UseCases;

public class RegistrarExecucaoUseCaseTests
{
    private readonly Mock<IExecucaoPedidoDataSource> _mockDS = new();

    private RegistrarExecucaoUseCase CriarUseCase()
        => RegistrarExecucaoUseCase.Create(ExecucaoPedidoGateway.Create(_mockDS.Object));

    [Fact]
    public async Task Run_QuandoNaoExiste_DeveCriarExecucao()
    {
        _mockDS.Setup(x => x.GetByPedidoId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExecucaoPedidoInputDTO?)null);
        _mockDS.Setup(x => x.Create(It.IsAny<ExecucaoPedidoInputDTO>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await CriarUseCase().Run(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);

        _mockDS.Verify(x => x.Create(It.IsAny<ExecucaoPedidoInputDTO>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Run_QuandoJaExiste_NaoDeveCriarNovamente()
    {
        var pedidoId = Guid.NewGuid();
        var dto = new ExecucaoPedidoInputDTO { Id = Guid.NewGuid(), PedidoId = pedidoId, Status = "Recebida", Etapas = new(), IdUsuarioCriacao = Guid.NewGuid(), DataCriacao = DateTime.UtcNow, Ativo = true };
        _mockDS.Setup(x => x.GetByPedidoId(pedidoId, It.IsAny<CancellationToken>())).ReturnsAsync(dto);

        await CriarUseCase().Run(pedidoId, Guid.NewGuid(), CancellationToken.None);

        _mockDS.Verify(x => x.Create(It.IsAny<ExecucaoPedidoInputDTO>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
