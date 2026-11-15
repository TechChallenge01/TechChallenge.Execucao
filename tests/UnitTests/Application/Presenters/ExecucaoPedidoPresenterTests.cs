using Application.Presenters.Execucao;
using Domain.Aggregates;
using FluentAssertions;
using System.Net;

namespace UnitTests.Application.Presenters;

public class ExecucaoPedidoPresenterTests
{
    private static ExecucaoPedido CriarExecucao() => new(Guid.NewGuid(), Guid.NewGuid());
    private static ExecucaoPedidoPresenter CriarPresenter() => new("Operação realizada");

    [Fact]
    public void TransformObject_DeveRetornarStatusOK()
    {
        var result = CriarPresenter().TransformObject(CriarExecucao());
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public void Created_DeveRetornarStatusCreated()
    {
        var id = Guid.NewGuid();
        var result = CriarPresenter().Created(id);
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public void NotFound_DeveRetornarStatusNotFound()
    {
        var result = CriarPresenter().NotFound("não encontrado");
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Message.Should().Be("não encontrado");
    }

    [Fact]
    public void BadRequest_DeveRetornarStatusBadRequest()
    {
        var result = CriarPresenter().BadRequest("erro");
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public void NoContent_DeveRetornarStatusNoContent()
    {
        var result = CriarPresenter().NoContent();
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
