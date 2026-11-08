namespace Shared.DTOs.Execucao.Output;

public class ExecucaoPedidoOutputDTO
{
    public Guid Id { get; set; }
    public Guid PedidoId { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal? ValorFinal { get; set; }
    public List<EtapaExecucaoOutputDTO> Etapas { get; set; } = new();
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}

public class EtapaExecucaoOutputDTO
{
    public string Status { get; set; } = string.Empty;
    public DateTime DataAlteracao { get; set; }
    public string? Observacao { get; set; }
}
