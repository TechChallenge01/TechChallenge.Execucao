using Domain.BaseEntity;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Aggregates;

public class ExecucaoPedido : Base
{
    public Guid Id { get; private set; }
    public Guid PedidoId { get; private set; }
    public EStatusExecucao Status { get; private set; }
    public decimal? ValorFinal { get; private set; }
    public List<EtapaExecucao> Etapas { get; private set; } = new();

    public ExecucaoPedido(Guid pedidoId, Guid idUsuario)
        : base(idUsuario, DateTime.UtcNow)
    {
        Id = Guid.NewGuid();
        PedidoId = pedidoId;
        Status = EStatusExecucao.Recebida;
        Etapas.Add(new EtapaExecucao(EStatusExecucao.Recebida.ToString(), DateTime.UtcNow));
    }

    public ExecucaoPedido(Guid id, Guid pedidoId, EStatusExecucao status, decimal? valorFinal,
        List<EtapaExecucao> etapas,
        Guid idUsuarioCriacao, DateTime dataCriacao,
        Guid? idUsuarioAtualizacao, DateTime? dataAtualizacao, bool ativo)
        : base(idUsuarioCriacao, dataCriacao, idUsuarioAtualizacao, dataAtualizacao)
    {
        Id = id;
        PedidoId = pedidoId;
        Status = status;
        ValorFinal = valorFinal;
        Etapas = etapas;
        Ativo = ativo;
    }

    public void IniciarPreparacao()
    {
        Status = EStatusExecucao.EmPreparacao;
        Etapas.Add(new EtapaExecucao(EStatusExecucao.EmPreparacao.ToString(), DateTime.UtcNow));
    }

    public void MarcarPronto()
    {
        Status = EStatusExecucao.Pronta;
        Etapas.Add(new EtapaExecucao(EStatusExecucao.Pronta.ToString(), DateTime.UtcNow));
    }

    public void Finalizar(decimal valorFinal)
    {
        Status = EStatusExecucao.Entregue;
        ValorFinal = valorFinal;
        Etapas.Add(new EtapaExecucao(EStatusExecucao.Entregue.ToString(), DateTime.UtcNow));
    }

    public void Falhar(string motivo)
    {
        Status = EStatusExecucao.Falhou;
        Etapas.Add(new EtapaExecucao(EStatusExecucao.Falhou.ToString(), DateTime.UtcNow, motivo));
    }
}
