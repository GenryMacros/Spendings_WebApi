namespace Spendings.Core.Exeptions
{
    public class FailedInsertionException : System.Exception
    {
        public FailedInsertionException(string message = "") : base(message) { }
    }
}
