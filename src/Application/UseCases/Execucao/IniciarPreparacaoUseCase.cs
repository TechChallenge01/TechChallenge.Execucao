using Application.Gateways.Execucao;
using Application.Interfaces;

namespace Application.UseCases.Execucao;

public class IniciarPreparacaoUseCase
{
    private readonly ExecucaoPedidoGateway _gateway;
    private readonly IEventPublisher _eventPublisher;

    private IniciarPreparacaoUseCase(ExecucaoPedidoGateway gateway, IEventPublisher eventPublisher)
    {
        _gateway = gateway;
        _eventPublisher = eventPublisher;
    }

    public static IniciarPreparacaoUseCase Create(ExecucaoPedidoGateway gateway, IEventPublisher eventPublisher)
        => new(gateway, eventPublisher);

    public async Task Run(Guid pedidoId, Guid idUsuario, CancellationToken ct)
    {
        var execucao = await _gateway.GetByPedidoId(pedidoId, ct);
        if (execucao is null) return;

        execucao.IniciarPreparacao();
        execucao.RastrearAlteracao(idUsuario, DateTime.UtcNow);
        await _gateway.Update(execucao, ct);

        await _eventPublisher.PublishAsync("execucao.pedido.em-preparacao", new
        {
            PedidoId = execucao.PedidoId,
            ExecucaoId = execucao.Id
        }, ct);
    }
}
