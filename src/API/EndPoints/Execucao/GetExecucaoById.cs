using API.Extensions;
using Application.Controllers.Execucao;
using Application.Interfaces;

namespace API.EndPoints.Execucao;

public class GetExecucaoById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/execucoes/{id:guid}", async (
            Guid id,
            IExecucaoPedidoDataSource dataSource,
            CancellationToken ct) =>
        {
            var controller = new ExecucaoController(dataSource);
            var result = await controller.GetById(id, ct);
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization()
        .WithTags("Execução")
        .WithSummary("Busca execução por ID");
    }
}
