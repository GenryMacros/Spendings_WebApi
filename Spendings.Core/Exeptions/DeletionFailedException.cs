namespace Spendings.Core.Exeptions
{
    public class DeletionFailedException : System.Exception
    {
        public DeletionFailedException(string message = "") : base(message) { }
    }
}
