using API.Extensions;
using Application.Controllers.Execucao;
using Application.Interfaces;

namespace API.EndPoints.Execucao;

public class GetExecucaoByPedidoId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/execucoes/pedido/{pedidoId:guid}", async (
            Guid pedidoId,
            IExecucaoPedidoDataSource dataSource,
            CancellationToken ct) =>
        {
            var controller = new ExecucaoController(dataSource);
            var result = await controller.GetByPedidoId(pedidoId, ct);
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization()
        .WithTags("Execução")
        .WithSummary("Busca execução pelo ID do pedido");
    }
}
