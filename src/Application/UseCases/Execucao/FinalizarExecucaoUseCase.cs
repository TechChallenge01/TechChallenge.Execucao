using Application.Gateways.Execucao;
using Application.Interfaces;
using Application.Presenters.Execucao;
using Shared.DTOs.Execucao.Request;
using Shared.Result;

namespace Application.UseCases.Execucao;

public class FinalizarExecucaoUseCase
{
    private readonly ExecucaoPedidoGateway _gateway;
    private readonly IEventPublisher _eventPublisher;
    private readonly ExecucaoPedidoPresenter _presenter = new("Execução finalizada com sucesso.");

    private FinalizarExecucaoUseCase(ExecucaoPedidoGateway gateway, IEventPublisher eventPublisher)
    {
        _gateway = gateway;
        _eventPublisher = eventPublisher;
    }

    public static FinalizarExecucaoUseCase Create(ExecucaoPedidoGateway gateway, IEventPublisher eventPublisher)
        => new(gateway, eventPublisher);

    public async Task<ICommandResult> Run(Guid id, FinalizarExecucaoRequestDTO request, Guid idUsuario, CancellationToken ct)
    {
        var execucao = await _gateway.GetById(id, ct);
        if (execucao is null)
            return _presenter.NotFound("Execução não encontrada.");

        execucao.Finalizar(request.ValorFinal);
        execucao.RastrearAlteracao(idUsuario, DateTime.UtcNow);
        await _gateway.Update(execucao, ct);

        await _eventPublisher.PublishAsync("execucao.finalizada", new
        {
            OrdemServicoId = execucao.PedidoId,
            request.ValorFinal,
            ExecucaoId = execucao.Id
        }, ct);

        return _presenter.TransformObject(execucao);
    }
}
