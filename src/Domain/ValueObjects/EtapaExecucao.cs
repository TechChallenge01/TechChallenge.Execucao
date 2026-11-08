namespace Domain.ValueObjects;

public class EtapaExecucao
{
    public EtapaExecucao(string status, DateTime dataAlteracao, string? observacao = null)
    {
        Status = status;
        DataAlteracao = dataAlteracao;
        Observacao = observacao;
    }

    public string Status { get; }
    public DateTime DataAlteracao { get; }
    public string? Observacao { get; }
}
