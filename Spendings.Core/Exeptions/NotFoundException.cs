namespace Spendings.Core.Exeptions
{
    public class NotFoundException : System.Exception
    {
        public NotFoundException(string message = "") : base(message) { }
    }
}
