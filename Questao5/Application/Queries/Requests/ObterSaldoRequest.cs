using MediatR;
using Questao5.Application.Queries.Responses;
using System.ComponentModel.DataAnnotations;

namespace Questao5.Application.Queries.Requests
{
    public class ObterSaldoRequest
        : IRequest<ObterSaldoResponse>
    {
        /// <summary>
        /// ID da conta corrente
        /// </summary>
        /// <example>382D323D-7067-ED11-8866-7D5DFA4A16C9</example>
        [Required]
        public Guid ContaCorrenteId { get; set; }
    }
}
