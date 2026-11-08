using Application.Interfaces;
using MongoDB.Driver;
using Shared.DTOs.Execucao.Input;

namespace Infra.DataSources;

public class ExecucaoPedidoDataSource : IExecucaoPedidoDataSource
{
    private readonly IMongoCollection<ExecucaoPedidoInputDTO> _collection;

    public ExecucaoPedidoDataSource(IMongoDatabase database)
        => _collection = database.GetCollection<ExecucaoPedidoInputDTO>("execucoes_pedido");

    public async Task<ExecucaoPedidoInputDTO?> GetById(Guid id, CancellationToken ct)
        => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(ct);

    public async Task<ExecucaoPedidoInputDTO?> GetByPedidoId(Guid pedidoId, CancellationToken ct)
        => await _collection.Find(x => x.PedidoId == pedidoId).FirstOrDefaultAsync(ct);

    public async Task Create(ExecucaoPedidoInputDTO dto, CancellationToken ct)
        => await _collection.InsertOneAsync(dto, cancellationToken: ct);

    public async Task Update(ExecucaoPedidoInputDTO dto, CancellationToken ct)
        => await _collection.ReplaceOneAsync(x => x.Id == dto.Id, dto, cancellationToken: ct);
}
