using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Questao5.Application.Exceptions;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Database.QueryStore.Handlers;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Sqlite;
using Questao5.Tests.Builders.ObterSaldo;
using Questao5.Tests.Fixtures;
using ServiceStack;
using System.Reflection;

namespace Questao5.Tests.Tests
{
    public class ObterSaldoHandlerTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _dbFixture;
        private readonly IServiceProvider _serviceProvider;

        public ObterSaldoHandlerTests(DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;
            IDbConnectionFactory dbConnMock = Substitute.For<IDbConnectionFactory>();
            dbConnMock.CreateConnection().Returns(_dbFixture.GetConnection());

            var services = new ServiceCollection();
            _serviceProvider = services
                .AddMediatR(Assembly.GetExecutingAssembly(), typeof(ObterSaldoHandler).Assembly)
                .AddSingleton(provider => dbConnMock)
                .AddScoped<IQueriesProcessor, QueriesProcessor>()
                .AddScoped(typeof(IQueryHandler<ObterSaldoRequest, ObterSaldoResponse>), typeof(ObterSaldoHandler))
                .AddScoped(typeof(IQueryHandler<ObterStatusContaCorrente, ObterStatusContaCorrenteResponse>), typeof(ObterStatusContaCorrenteHandler))
                .AddScoped(typeof(IQueryHandler<ObterSaldoPorContaId, ObterSaldoPorContaIdResponse>), typeof(ObterSaldoPorContaIdHandler))
                .BuildServiceProvider();
        }

        [Theory]
        [InlineData("B6BAFC09-6967-ED11-A567-055DFA4A16C9")]
        [InlineData("FA99D033-7067-ED11-96C6-7C5DFA4A16C9")]
        [InlineData("382D323D-7067-ED11-8866-7D5DFA4A16C9")]
        public async Task Handle_Should_ReturnCorrectly_When_ObterSaldo(Guid contaCorrenteId)
        {
            var obterSaldoRequest = new ObterSaldoRequestBuilder()
                .WithContaCorrenteId(contaCorrenteId)
                .Build();

            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            _dbFixture.Insert(new List<Movimento>
            {
                new Movimento
                {
                    IdContaCorrente = contaCorrenteId.ToString().ToUpper(),
                    IdMovimento = Guid.NewGuid().ToString().ToUpper(),
                    Valor = 100,
                    TipoMovimento = "C"
                }
            });


            var result = await mediator.Send(obterSaldoRequest);
            result.Numero.Should().NotBeNullOrEmpty();
            result.Titular.Should().NotBeNullOrEmpty();
            result.Saldo.Should().Be(100);
        }

        [Theory]
        [InlineData("af9e5bd7-e86f-4b90-aa48-a760dd1ee992")]
        [InlineData("5a7359ef-4082-4d19-a987-2ecda8467b65")]
        [InlineData("074c3791-ed18-4ce9-993c-fb7196618fe7")]
        public async Task Handle_Should_ReturnException_When_ContaCorrenteDoesntExists(Guid contaCorrenteId)
        {
            var obterSaldoRequest = new ObterSaldoRequestBuilder()
                .WithContaCorrenteId(contaCorrenteId)
                .Build();

            var mediator = _serviceProvider.GetRequiredService<IMediator>();

            await mediator.Invoking(m => m.Send(obterSaldoRequest))
                          .Should()
                          .ThrowExactlyAsync<InvalidAccountException>()
                          .WithMessage("Apenas contas correntes cadastradas podem consultar o saldo.");
        }

        [Theory]
        [InlineData("F475F943-7067-ED11-A06B-7E5DFA4A16C9")]
        [InlineData("BCDACA4A-7067-ED11-AF81-825DFA4A16C9")]
        [InlineData("D2E02051-7067-ED11-94C0-835DFA4A16C9")]
        public async Task Handle_Should_ReturnException_When_ContaCorrenteIsInactive(Guid contaCorrenteId)
        {
            var obterSaldoRequest = new ObterSaldoRequestBuilder()
                .WithContaCorrenteId(contaCorrenteId)
                .Build();

            var mediator = _serviceProvider.GetRequiredService<IMediator>();

            await mediator.Invoking(m => m.Send(obterSaldoRequest))
                          .Should()
                          .ThrowExactlyAsync<InactiveAccountException>()
                          .WithMessage("Apenas contas correntes ativas podem consultar o saldo.");
        }
    }
}
