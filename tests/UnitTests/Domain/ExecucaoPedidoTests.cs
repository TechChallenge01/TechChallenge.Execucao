using Domain.Aggregates;
using Domain.Enums;
using FluentAssertions;

namespace UnitTests.Domain;

public class ExecucaoPedidoTests
{
    private static ExecucaoPedido CriarExecucao()
        => new(Guid.NewGuid(), Guid.NewGuid());

    [Fact]
    public void CriarExecucao_DeveIniciarComStatusRecebida()
    {
        var execucao = CriarExecucao();

        execucao.Status.Should().Be(EStatusExecucao.Recebida);
    }

    [Fact]
    public void CriarExecucao_DeveAdicionarPrimeiraEtapa()
    {
        var execucao = CriarExecucao();

        execucao.Etapas.Should().HaveCount(1);
        execucao.Etapas[0].Status.Should().Be(EStatusExecucao.Recebida.ToString());
    }

    [Fact]
    public void CriarExecucao_DeveGerarId()
    {
        var execucao = CriarExecucao();

        execucao.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void IniciarPreparacao_DeveMudarStatus()
    {
        var execucao = CriarExecucao();

        execucao.IniciarPreparacao();

        execucao.Status.Should().Be(EStatusExecucao.EmPreparacao);
    }

    [Fact]
    public void IniciarPreparacao_DeveAdicionarEtapa()
    {
        var execucao = CriarExecucao();

        execucao.IniciarPreparacao();

        execucao.Etapas.Should().HaveCount(2);
        execucao.Etapas.Last().Status.Should().Be(EStatusExecucao.EmPreparacao.ToString());
    }

    [Fact]
    public void MarcarPronto_DeveMudarStatus()
    {
        var execucao = CriarExecucao();
        execucao.IniciarPreparacao();

        execucao.MarcarPronto();

        execucao.Status.Should().Be(EStatusExecucao.Pronta);
    }

    [Fact]
    public void MarcarPronto_DeveAdicionarEtapa()
    {
        var execucao = CriarExecucao();
        execucao.IniciarPreparacao();

        execucao.MarcarPronto();

        execucao.Etapas.Should().HaveCount(3);
    }

    [Fact]
    public void Finalizar_DeveDefinirValorFinalEStatus()
    {
        var execucao = CriarExecucao();

        execucao.Finalizar(1500m);

        execucao.Status.Should().Be(EStatusExecucao.Entregue);
        execucao.ValorFinal.Should().Be(1500m);
    }

    [Fact]
    public void Finalizar_DeveAdicionarEtapa()
    {
        var execucao = CriarExecucao();

        execucao.Finalizar(800m);

        execucao.Etapas.Last().Status.Should().Be(EStatusExecucao.Entregue.ToString());
    }

    [Fact]
    public void Falhar_DeveMudarStatusParaFalhou()
    {
        var execucao = CriarExecucao();

        execucao.Falhar("Peça indisponível");

        execucao.Status.Should().Be(EStatusExecucao.Falhou);
    }

    [Fact]
    public void Falhar_DeveAdicionarEtapaComMotivo()
    {
        var execucao = CriarExecucao();
        const string motivo = "Peça indisponível no estoque";

        execucao.Falhar(motivo);

        var ultimaEtapa = execucao.Etapas.Last();
        ultimaEtapa.Status.Should().Be(EStatusExecucao.Falhou.ToString());
        ultimaEtapa.Observacao.Should().Be(motivo);
    }
}
