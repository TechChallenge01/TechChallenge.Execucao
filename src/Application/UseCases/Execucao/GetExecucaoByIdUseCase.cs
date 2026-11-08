using Application.Gateways.Execucao;
using Application.Presenters.Execucao;
using Shared.Result;

namespace Application.UseCases.Execucao;

public class GetExecucaoByIdUseCase
{
    private readonly ExecucaoPedidoGateway _gateway;
    private readonly ExecucaoPedidoPresenter _presenter = new("Execução encontrada.");

    private GetExecucaoByIdUseCase(ExecucaoPedidoGateway gateway) => _gateway = gateway;

    public static GetExecucaoByIdUseCase Create(ExecucaoPedidoGateway gateway) => new(gateway);

    public async Task<ICommandResult> Run(Guid id, CancellationToken ct)
    {
        var execucao = await _gateway.GetById(id, ct);
        return execucao is null ? _presenter.NotFound("Execução não encontrada.") : _presenter.TransformObject(execucao);
    }
}
