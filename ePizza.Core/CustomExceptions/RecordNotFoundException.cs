namespace ePizza.Core.CustomExceptions
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException(string message): base(message) { }
    }
}
