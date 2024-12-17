using Dapper;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Questao5.Application.Exceptions;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.CommandStore.Handlers;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Database.QueryStore.Handlers;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Sqlite;
using Questao5.Tests.Builders;
using Questao5.Tests.Fixtures;
using System.Reflection;
using System.Text.Json;

namespace Questao5.Tests.Tests
{
    public class RealizarMovimentoHandlerTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _dbFixture;
        private readonly IServiceProvider _serviceProvider;
        public RealizarMovimentoHandlerTests(DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;
            IDbConnectionFactory dbConnMock = Substitute.For<IDbConnectionFactory>();
            dbConnMock.CreateConnection().Returns(_dbFixture.GetConnection());

            var services = new ServiceCollection();
            _serviceProvider = services
                .AddMediatR(Assembly.GetExecutingAssembly(), typeof(RealizarMovimentoHandler).Assembly)
                .AddSingleton(provider => dbConnMock)
                .AddScoped<IQueriesProcessor, QueriesProcessor>()
                .AddScoped<ICommandsProcessor, CommandsProcessor>()
                .AddScoped(typeof(IQueryHandler<ObterIdempotencia, ObterIdempotenciaResponse>), typeof(ObterIdempotenciaHandler))
                .AddScoped(typeof(IQueryHandler<ObterStatusContaCorrente, ObterStatusContaCorrenteResponse>), typeof(ObterStatusContaCorrenteHandler))
                .AddScoped(typeof(ICommandHandler<CriarMovimento>), typeof(CriarMovimentoHandler))
                .BuildServiceProvider();
        }

        [Theory]
        [InlineData("Tipo errado")]
        [InlineData("Tipo definitivamente errado")]
        [InlineData("guilherme")]
        [InlineData("a")]
        [InlineData("1")] 
        [InlineData(" ")] 
        [InlineData("")]
        public async Task Handler_Should_ReturnException_WhenTipoIsWrong(string tipo)
        {
            var mediator = _serviceProvider.GetService<IMediator>();

            var realizarMovimento = new RealizarMovimentoRequestBuilder()
                .WithRequisicaoId(Guid.NewGuid())
                .WithContaCorrenteId(Guid.NewGuid())
                .WithValor(100)
                .WithTipo(tipo)
                .Build();

            await mediator.Invoking(m => m.Send(realizarMovimento))
                .Should()
                .ThrowExactlyAsync<InvalidTypeException>()
                .WithMessage("Apenas os tipos 'débito' (D) ou 'crédito' (C) podem ser aceitos.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Handler_Should_ReturnException_WhenValorIsLessOrEqualZero(decimal valor)
        {
            var mediator = _serviceProvider.GetService<IMediator>();

            var realizarMovimento = new RealizarMovimentoRequestBuilder()
                .WithRequisicaoId(Guid.NewGuid())
                .WithContaCorrenteId(Guid.NewGuid())
                .WithValor(valor)
                .WithTipo("C")
                .Build();

            await mediator.Invoking(m => m.Send(realizarMovimento))
                .Should()
                .ThrowExactlyAsync<InvalidValueException>()
                .WithMessage("Apenas valores positivos podem ser recebidos.");
        }

        [Fact]
        public async Task Handler_Should_ReturnException_WhenContaCorrenteDoesntExist()
        {
            var mediator = _serviceProvider.GetService<IMediator>();

            var realizarMovimento = new RealizarMovimentoRequestBuilder()
                .WithRequisicaoId(Guid.NewGuid())
                .WithContaCorrenteId(Guid.NewGuid())
                .WithValor(100)
                .WithTipo("C")
                .Build();

            await mediator.Invoking(m => m.Send(realizarMovimento))
                .Should()
                .ThrowExactlyAsync<InvalidAccountException>()
                .WithMessage("Apenas contas correntes cadastradas podem receber movimentação.");
        }

        [Fact]
        public async Task Handler_Should_ReturnException_WhenContaCorrenteIsInactive()
        {
            var mediator = _serviceProvider.GetService<IMediator>();

            var realizarMovimento = new RealizarMovimentoRequestBuilder()
                .WithRequisicaoId(Guid.NewGuid())
                .WithContaCorrenteId(Guid.Parse("F475F943-7067-ED11-A06B-7E5DFA4A16C9"))
                .WithValor(100)
                .WithTipo("C")
                .Build();

            await mediator.Invoking(m => m.Send(realizarMovimento))
                .Should()
                .ThrowExactlyAsync<InactiveAccountException>()
                .WithMessage("Apenas contas correntes ativas podem receber movimentação.");
        }

        [Theory]
        [InlineData("B6BAFC09-6967-ED11-A567-055DFA4A16C9")]
        [InlineData("FA99D033-7067-ED11-96C6-7C5DFA4A16C9")]
        [InlineData("382D323D-7067-ED11-8866-7D5DFA4A16C9")]
        public async Task Handler_Should_CreateMovimento_Correctly(Guid contaCorrenteId)
        {
            var mediator = _serviceProvider.GetService<IMediator>();

            var realizarMovimento = new RealizarMovimentoRequestBuilder()
                .WithRequisicaoId(Guid.NewGuid())
                .WithContaCorrenteId(contaCorrenteId)
                .WithValor(100)
                .WithTipo("C")
                .Build();

            var result = await mediator.Send(realizarMovimento);

            using var conn = _dbFixture.GetConnection();
            var movimento = await conn.QuerySingleOrDefaultAsync<Movimento>("SELECT * FROM movimento WHERE idcontacorrente = @contaCorrenteId", new { contaCorrenteId = contaCorrenteId.ToString().ToUpper() });
            movimento.IdMovimento.Should().Be(result.MovimentoId);
        }

        [Fact]
        public async Task Handler_Should_ReturnException_WhenMovimentoIsNotProcessed()
        {
            var mediator = _serviceProvider.GetService<IMediator>();
            var idempotencyId = Guid.NewGuid();
            var realizarMovimento = new RealizarMovimentoRequestBuilder()
                .WithRequisicaoId(idempotencyId)
                .WithContaCorrenteId(Guid.Parse("B6BAFC09-6967-ED11-A567-055DFA4A16C9"))
                .WithValor(100)
                .WithTipo("C")
                .Build();

            _dbFixture.Insert(new List<Idempotencia>
            {
                new Idempotencia
                {
                    Chave_Idempotencia = idempotencyId.ToString().ToUpper(),
                    Requisicao = JsonSerializer.Serialize(realizarMovimento),
                    Resultado = string.Empty
                }
            });

            await mediator.Invoking(m => m.Send(realizarMovimento))
                .Should()
                .ThrowExactlyAsync<PendingIdempotency>()
                .WithMessage("Sua requisição está sendo processada.");
        }
    }
}
