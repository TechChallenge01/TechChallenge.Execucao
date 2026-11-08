using System.Net;
using Domain.Aggregates;
using Shared.DTOs.Execucao.Output;
using Shared.Result;

namespace Application.Presenters.Execucao;

public class ExecucaoPedidoPresenter
{
    private readonly string _message;

    public ExecucaoPedidoPresenter(string message) => _message = message;

    public ICommandResult<ExecucaoPedidoOutputDTO> TransformObject(ExecucaoPedido execucao)
        => new CommandResult<ExecucaoPedidoOutputDTO>(_message, HttpStatusCode.OK, Transform(execucao));

    public ICommandResult<T> Created<T>(T data)
        => new CommandResult<T>(_message, HttpStatusCode.Created, data);

    public ICommandResult NotFound(string? message = null)
        => new CommandResult(message ?? _message, HttpStatusCode.NotFound);

    public ICommandResult BadRequest(string? message = null)
        => new CommandResult(message ?? _message, HttpStatusCode.BadRequest);

    public ICommandResult NoContent()
        => new CommandResult(_message, HttpStatusCode.NoContent);

    private static ExecucaoPedidoOutputDTO Transform(ExecucaoPedido e) => new()
    {
        Id = e.Id,
        PedidoId = e.PedidoId,
        Status = e.Status.ToString(),
        ValorFinal = e.ValorFinal,
        Etapas = e.Etapas.Select(et => new EtapaExecucaoOutputDTO
        {
            Status = et.Status,
            DataAlteracao = et.DataAlteracao,
            Observacao = et.Observacao
        }).ToList(),
        DataCriacao = e.DataCriacao,
        DataAtualizacao = e.DataAtualizacao
    };
}
