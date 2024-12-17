namespace Questao1
{
    class ContaBancaria
    {
        // Ficaria em um lugar de configuração ou variáveis.
        private double TaxaSaque = 3.5d;
        public int Numero { get; private set; }
        public double Saldo { get; private set; }

        // Optei por deixar assim já que a troca do titular não é realizada, o ideal seria um método para trocar o titular + logs das trocas.
        public string Titular { get; set; }


        public ContaBancaria(int numero, string titular, double depositoInicial = 0)
        {
            Numero = numero;
            Titular = titular;
            Saldo = depositoInicial;
        }

        public void Depositar(double quantia)
        {
            Saldo += quantia;
        }

        public void Sacar(double quantia)
        {
            var valorSaqueMaisTaxa = quantia + TaxaSaque;
            Saldo -= valorSaqueMaisTaxa;
        }

        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {Saldo.ToString("0.00")}";
        }
    }
}
