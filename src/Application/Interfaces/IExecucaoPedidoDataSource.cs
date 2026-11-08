using Shared.DTOs.Execucao.Input;

namespace Application.Interfaces;

public interface IExecucaoPedidoDataSource
{
    Task<ExecucaoPedidoInputDTO?> GetById(Guid id, CancellationToken ct);
    Task<ExecucaoPedidoInputDTO?> GetByPedidoId(Guid pedidoId, CancellationToken ct);
    Task Create(ExecucaoPedidoInputDTO dto, CancellationToken ct);
    Task Update(ExecucaoPedidoInputDTO dto, CancellationToken ct);
}
