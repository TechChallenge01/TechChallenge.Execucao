using Application.Gateways.Execucao;
using Application.Interfaces;
using Application.UseCases.Execucao;
using FluentAssertions;
using Moq;
using Shared.DTOs.Execucao.Input;
using System.Net;

namespace UnitTests.Application.UseCases;

public class MarcarProntoUseCaseTests
{
    private readonly Mock<IExecucaoPedidoDataSource> _mockDS = new();
    private readonly Mock<IEventPublisher> _mockPublisher = new();

    private MarcarProntoUseCase CriarUseCase()
        => MarcarProntoUseCase.Create(ExecucaoPedidoGateway.Create(_mockDS.Object), _mockPublisher.Object);

    private static ExecucaoPedidoInputDTO ExecucaoDTO() => new()
    {
        Id = Guid.NewGuid(), PedidoId = Guid.NewGuid(), Status = "EmPreparacao",
        Etapas = new() { new() { Status = "EmPreparacao", DataAlteracao = DateTime.UtcNow } },
        IdUsuarioCriacao = Guid.NewGuid(), DataCriacao = DateTime.UtcNow, Ativo = true
    };

    [Fact]
    public async Task Run_QuandoExiste_DeveMarcarProntoEPublicar()
    {
        var dto = ExecucaoDTO();
        _mockDS.Setup(x => x.GetById(dto.Id, It.IsAny<CancellationToken>())).ReturnsAsync(dto);
        _mockDS.Setup(x => x.Update(It.IsAny<ExecucaoPedidoInputDTO>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockPublisher.Setup(x => x.PublishAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await CriarUseCase().Run(dto.Id, Guid.NewGuid(), CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        _mockPublisher.Verify(x => x.PublishAsync("execucao.pedido.pronto", It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Run_QuandoNaoExiste_DeveRetornarNotFound()
    {
        _mockDS.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExecucaoPedidoInputDTO?)null);

        var result = await CriarUseCase().Run(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
