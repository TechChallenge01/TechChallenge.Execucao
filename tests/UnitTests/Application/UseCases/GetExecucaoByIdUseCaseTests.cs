using Application.Gateways.Execucao;
using Application.Interfaces;
using Application.UseCases.Execucao;
using FluentAssertions;
using Moq;
using Shared.DTOs.Execucao.Input;
using System.Net;

namespace UnitTests.Application.UseCases;

public class GetExecucaoByIdUseCaseTests
{
    private readonly Mock<IExecucaoPedidoDataSource> _mockDS = new();

    private GetExecucaoByIdUseCase CriarUseCase()
        => GetExecucaoByIdUseCase.Create(ExecucaoPedidoGateway.Create(_mockDS.Object));

    [Fact]
    public async Task Run_QuandoExiste_DeveRetornarOk()
    {
        var dto = new ExecucaoPedidoInputDTO { Id = Guid.NewGuid(), PedidoId = Guid.NewGuid(), Status = "Recebida", Etapas = new(), IdUsuarioCriacao = Guid.NewGuid(), DataCriacao = DateTime.UtcNow, Ativo = true };
        _mockDS.Setup(x => x.GetById(dto.Id, It.IsAny<CancellationToken>())).ReturnsAsync(dto);

        var result = await CriarUseCase().Run(dto.Id, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Run_QuandoNaoExiste_DeveRetornarNotFound()
    {
        _mockDS.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExecucaoPedidoInputDTO?)null);

        var result = await CriarUseCase().Run(Guid.NewGuid(), CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
