using MediatR;
using Questao5.Application.Commands.Responses;
using System.ComponentModel.DataAnnotations;

namespace Questao5.Application.Commands.Requests
{
    public class RealizarMovimentoRequest
        : IRequest<RealizarMovimentoResponse>
    {
        /// <summary>
        /// ID da requisição para fins de idempotencia, caso já foi utilizado esse ID retorna o resultado que foi obtido.
        /// </summary>
        /// <example>a926eb91-3e5a-4a5c-a46f-417f7e6b3b33</example>
        [Required]
        public Guid RequisicaoId { get; set; }

        /// <summary>
        /// ID da conta corrente
        /// </summary>
        /// <example>382D323D-7067-ED11-8866-7D5DFA4A16C9</example>
        [Required]
        public Guid ContaCorrenteId { get; set; }

        /// <summary>
        /// Valor do movimento
        /// </summary>
        /// <example>300.5</example>
        [Required]
        public decimal Valor { get; set; }

        /// <summary>
        /// Tipo do movimento
        /// </summary>
        /// <example>C</example>
        [Required]
        public string Tipo { get; set; }
    }
}
