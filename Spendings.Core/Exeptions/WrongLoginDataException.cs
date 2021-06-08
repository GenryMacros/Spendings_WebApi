namespace Spendings.Core.Exeptions
{
    public class WrongLoginDataException : System.Exception
    {
        public WrongLoginDataException(string message = "") : base(message) {}
    }
}
