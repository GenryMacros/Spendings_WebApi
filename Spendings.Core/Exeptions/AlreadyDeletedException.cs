namespace Spendings.Core.Exeptions
{
    public class AlreadyDeletedException : System.Exception
    {
        public AlreadyDeletedException(string message = "") : base(message) { }
    }
}
