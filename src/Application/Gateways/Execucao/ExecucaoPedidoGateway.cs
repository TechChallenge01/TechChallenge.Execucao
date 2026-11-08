using Application.Interfaces;
using Domain.Aggregates;
using Domain.Enums;
using Domain.ValueObjects;
using Shared.DTOs.Execucao.Input;

namespace Application.Gateways.Execucao;

public class ExecucaoPedidoGateway
{
    private readonly IExecucaoPedidoDataSource _dataSource;

    private ExecucaoPedidoGateway(IExecucaoPedidoDataSource dataSource) => _dataSource = dataSource;

    public static ExecucaoPedidoGateway Create(IExecucaoPedidoDataSource dataSource) => new(dataSource);

    public async Task<ExecucaoPedido?> GetById(Guid id, CancellationToken ct)
    {
        var dto = await _dataSource.GetById(id, ct);
        return dto is null ? null : Reconstituir(dto);
    }

    public async Task<ExecucaoPedido?> GetByPedidoId(Guid pedidoId, CancellationToken ct)
    {
        var dto = await _dataSource.GetByPedidoId(pedidoId, ct);
        return dto is null ? null : Reconstituir(dto);
    }

    public async Task Create(ExecucaoPedido execucao, CancellationToken ct)
        => await _dataSource.Create(Serializar(execucao), ct);

    public async Task Update(ExecucaoPedido execucao, CancellationToken ct)
        => await _dataSource.Update(Serializar(execucao), ct);

    private static ExecucaoPedido Reconstituir(ExecucaoPedidoInputDTO dto) =>
        new(dto.Id, dto.PedidoId,
            Enum.Parse<EStatusExecucao>(dto.Status),
            dto.ValorFinal,
            dto.Etapas.Select(e => new EtapaExecucao(e.Status, e.DataAlteracao, e.Observacao)).ToList(),
            dto.IdUsuarioCriacao, dto.DataCriacao,
            dto.IdUsuarioAtualizacao, dto.DataAtualizacao, dto.Ativo);

    private static ExecucaoPedidoInputDTO Serializar(ExecucaoPedido e) => new()
    {
        Id = e.Id,
        PedidoId = e.PedidoId,
        Status = e.Status.ToString(),
        ValorFinal = e.ValorFinal,
        Etapas = e.Etapas.Select(et => new EtapaExecucaoInputDTO
        {
            Status = et.Status,
            DataAlteracao = et.DataAlteracao,
            Observacao = et.Observacao
        }).ToList(),
        IdUsuarioCriacao = e.IdUsuarioCriacao,
        DataCriacao = e.DataCriacao,
        IdUsuarioAtualizacao = e.IdUsuarioAtualizacao,
        DataAtualizacao = e.DataAtualizacao,
        Ativo = e.Ativo
    };
}
