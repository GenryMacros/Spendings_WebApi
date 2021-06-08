namespace Spendings.Core.Exeptions
{
    public class EmptyIntervalException : System.Exception
    {
        public EmptyIntervalException(string message = "") : base(message) { }
    }
}
