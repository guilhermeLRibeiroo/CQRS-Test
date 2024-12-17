namespace Questao5.Application.Exceptions
{
    public class PendingIdempotency : Exception
    {
        public PendingIdempotency(string message) : base(message) { }
    }
}
