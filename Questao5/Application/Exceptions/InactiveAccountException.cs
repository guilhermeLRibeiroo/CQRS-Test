namespace Questao5.Application.Exceptions
{
    public class InactiveAccountException : Exception
    {
        public InactiveAccountException(string message) : base(message) { }
    }
}
