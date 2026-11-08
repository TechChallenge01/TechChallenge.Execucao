using Application.Gateways.Execucao;
using Application.Presenters.Execucao;
using Shared.Result;

namespace Application.UseCases.Execucao;

public class GetExecucaoByPedidoIdUseCase
{
    private readonly ExecucaoPedidoGateway _gateway;
    private readonly ExecucaoPedidoPresenter _presenter = new("Execução encontrada.");

    private GetExecucaoByPedidoIdUseCase(ExecucaoPedidoGateway gateway) => _gateway = gateway;

    public static GetExecucaoByPedidoIdUseCase Create(ExecucaoPedidoGateway gateway) => new(gateway);

    public async Task<ICommandResult> Run(Guid pedidoId, CancellationToken ct)
    {
        var execucao = await _gateway.GetByPedidoId(pedidoId, ct);
        return execucao is null ? _presenter.NotFound("Execução não encontrada para este pedido.") : _presenter.TransformObject(execucao);
    }
}
