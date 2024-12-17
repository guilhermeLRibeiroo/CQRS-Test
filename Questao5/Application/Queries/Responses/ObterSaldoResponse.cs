namespace Questao5.Application.Queries.Responses
{
    public class ObterSaldoResponse
    {
        /// <summary>
        /// Numero da conta
        /// </summary>
        /// <example>789</example>
        public string Numero { get; set; }

        /// <summary>
        /// Nome do titular
        /// </summary>
        /// <example>Tevin Mcconnell</example>
        public string Titular { get; set; }

        /// <summary>
        /// Data e Hora da requisição UTC
        /// </summary>
        /// <example>15/12/2024 04:28:42</example>
        public string DataHora { get; set; } = DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm:ss");

        /// <summary>
        /// Saldo total da conta
        /// </summary>
        /// <example>-450</example>
        public decimal Saldo { get; set; }
    }
}
