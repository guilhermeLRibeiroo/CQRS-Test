using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Exceptions;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Infrastructure.Documentation;
using System.Net.Mime;

namespace Questao5.Infrastructure.Services.Controllers
{
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    public class ContaCorrenteController : ControllerBase
    {
        /// <summary>
        /// Pesquisa de saldo.
        /// </summary>
        /// <remarks>
        /// Pesquisa o saldo, titular e numero da conta no banco de dados.
        /// </remarks>
        /// <param name="command">Objeto contendo o Id da conta corrente</param>
        /// <param name="handler">Objeto do MediatR que serve como o design pattern mediator</param>
        /// <response code="400">Apenas contas correntes cadastradas podem consultar o saldo.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ObterSaldoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ObterSaldoExceptionExample), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(
            [FromServices] IMediator handler,
            [FromQuery, BindRequired] ObterSaldoRequest command)
        {
            try
            {
                return Ok(await handler.Send(command));
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = $"{ex.GetType().Name}: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Cadastro de movimento.
        /// </summary>
        /// <remarks>
        /// Cadastra um movimento baseado na conta corrente.
        /// </remarks>
        /// <param name="command">Objeto contendo o RequisicaoId, ContaCorrenteId, Valor e Tipo do movimento.</param>
        /// <param name="handler">Objeto do MediatR que serve como o design pattern mediator</param>
        /// <response code="400">Apenas os tipos 'débito' (D) ou 'crédito' (C) podem ser aceitos.</response>
        [HttpPost]
        [ProducesResponseType(typeof(RealizarMovimentoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RealizarMovimentoExceptionExample), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(
            [FromServices] IMediator handler,
            [FromBody, BindRequired] RealizarMovimentoRequest command)
        {
            try
            {
                return Ok(await handler.Send(command));
            }
            catch (PendingIdempotency ex)
            {
                return Accepted(value: new
                {
                    message = $"{ex.GetType().Name}: {ex.Message}"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = $"{ex.GetType().Name}: {ex.Message}"
                });
            }
        }
    }
}
