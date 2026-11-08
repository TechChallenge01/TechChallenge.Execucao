using Application.Gateways.Execucao;
using Application.Interfaces;
using Application.UseCases.Execucao;
using Shared.DTOs.Execucao.Request;
using Shared.Result;

namespace Application.Controllers.Execucao;

public class ExecucaoController
{
    private readonly IExecucaoPedidoDataSource _dataSource;

    public ExecucaoController(IExecucaoPedidoDataSource dataSource) => _dataSource = dataSource;

    public Task RegistrarExecucao(Guid pedidoId, Guid idUsuario, CancellationToken ct)
        => RegistrarExecucaoUseCase.Create(ExecucaoPedidoGateway.Create(_dataSource)).Run(pedidoId, idUsuario, ct);

    public Task IniciarPreparacao(Guid pedidoId, Guid idUsuario, IEventPublisher eventPublisher, CancellationToken ct)
        => IniciarPreparacaoUseCase.Create(ExecucaoPedidoGateway.Create(_dataSource), eventPublisher).Run(pedidoId, idUsuario, ct);

    public Task<ICommandResult> MarcarPronto(Guid id, Guid idUsuario, IEventPublisher eventPublisher, CancellationToken ct)
        => MarcarProntoUseCase.Create(ExecucaoPedidoGateway.Create(_dataSource), eventPublisher).Run(id, idUsuario, ct);

    public Task<ICommandResult> Finalizar(Guid id, FinalizarExecucaoRequestDTO request, Guid idUsuario, IEventPublisher eventPublisher, CancellationToken ct)
        => FinalizarExecucaoUseCase.Create(ExecucaoPedidoGateway.Create(_dataSource), eventPublisher).Run(id, request, idUsuario, ct);

    public Task<ICommandResult> GetById(Guid id, CancellationToken ct)
        => GetExecucaoByIdUseCase.Create(ExecucaoPedidoGateway.Create(_dataSource)).Run(id, ct);

    public Task<ICommandResult> GetByPedidoId(Guid pedidoId, CancellationToken ct)
        => GetExecucaoByPedidoIdUseCase.Create(ExecucaoPedidoGateway.Create(_dataSource)).Run(pedidoId, ct);
}
