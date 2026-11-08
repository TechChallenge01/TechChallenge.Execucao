using API.Extensions;
using Application.Controllers.Execucao;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Execucao.Request;

namespace API.EndPoints.Execucao;

public class FinalizarExecucao : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/execucoes/{id:guid}/finalizar", async (
            Guid id,
            [FromBody] FinalizarExecucaoRequestDTO request,
            HttpContext ctx,
            IExecucaoPedidoDataSource dataSource,
            IEventPublisher eventPublisher,
            CancellationToken ct) =>
        {
            var idUsuario = ctx.User.GetUserId();
            var controller = new ExecucaoController(dataSource);
            var result = await controller.Finalizar(id, request, idUsuario, eventPublisher, ct);
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization()
        .WithTags("Execução")
        .WithSummary("Finaliza e entrega a execução");
    }
}
