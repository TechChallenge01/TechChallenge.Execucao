using Application.Gateways.Execucao;
using Application.Interfaces;
using Application.Presenters.Execucao;
using Shared.Result;

namespace Application.UseCases.Execucao;

public class MarcarProntoUseCase
{
    private readonly ExecucaoPedidoGateway _gateway;
    private readonly IEventPublisher _eventPublisher;
    private readonly ExecucaoPedidoPresenter _presenter = new("Pedido marcado como pronto.");

    private MarcarProntoUseCase(ExecucaoPedidoGateway gateway, IEventPublisher eventPublisher)
    {
        _gateway = gateway;
        _eventPublisher = eventPublisher;
    }

    public static MarcarProntoUseCase Create(ExecucaoPedidoGateway gateway, IEventPublisher eventPublisher)
        => new(gateway, eventPublisher);

    public async Task<ICommandResult> Run(Guid id, Guid idUsuario, CancellationToken ct)
    {
        var execucao = await _gateway.GetById(id, ct);
        if (execucao is null)
            return _presenter.NotFound("Execução não encontrada.");

        execucao.MarcarPronto();
        execucao.RastrearAlteracao(idUsuario, DateTime.UtcNow);
        await _gateway.Update(execucao, ct);

        await _eventPublisher.PublishAsync("execucao.pedido.pronto", new
        {
            PedidoId = execucao.PedidoId,
            ExecucaoId = execucao.Id
        }, ct);

        return _presenter.TransformObject(execucao);
    }
}
