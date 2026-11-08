using Application.Gateways.Execucao;
using Domain.Aggregates;

namespace Application.UseCases.Execucao;

public class RegistrarExecucaoUseCase
{
    private readonly ExecucaoPedidoGateway _gateway;

    private RegistrarExecucaoUseCase(ExecucaoPedidoGateway gateway) => _gateway = gateway;

    public static RegistrarExecucaoUseCase Create(ExecucaoPedidoGateway gateway) => new(gateway);

    public async Task Run(Guid pedidoId, Guid idUsuario, CancellationToken ct)
    {
        var existente = await _gateway.GetByPedidoId(pedidoId, ct);
        if (existente is not null) return;

        var execucao = new ExecucaoPedido(pedidoId, idUsuario);
        await _gateway.Create(execucao, ct);
    }
}
