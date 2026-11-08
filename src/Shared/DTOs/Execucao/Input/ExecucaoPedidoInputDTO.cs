namespace Shared.DTOs.Execucao.Input;

public class ExecucaoPedidoInputDTO
{
    public Guid Id { get; set; }
    public Guid PedidoId { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal? ValorFinal { get; set; }
    public List<EtapaExecucaoInputDTO> Etapas { get; set; } = new();
    public Guid IdUsuarioCriacao { get; set; }
    public DateTime DataCriacao { get; set; }
    public Guid? IdUsuarioAtualizacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public bool Ativo { get; set; }
}

public class EtapaExecucaoInputDTO
{
    public string Status { get; set; } = string.Empty;
    public DateTime DataAlteracao { get; set; }
    public string? Observacao { get; set; }
}
