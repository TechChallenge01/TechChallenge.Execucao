using API.Extensions;
using Application.Controllers.Execucao;
using Application.Interfaces;

namespace API.EndPoints.Execucao;

public class MarcarPronto : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/execucoes/{id:guid}/marcar-pronto", async (
            Guid id,
            HttpContext ctx,
            IExecucaoPedidoDataSource dataSource,
            IEventPublisher eventPublisher,
            CancellationToken ct) =>
        {
            var idUsuario = ctx.User.GetUserId();
            var controller = new ExecucaoController(dataSource);
            var result = await controller.MarcarPronto(id, idUsuario, eventPublisher, ct);
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization()
        .WithTags("Execução")
        .WithSummary("Marca a execução como pronta para entrega");
    }
}
