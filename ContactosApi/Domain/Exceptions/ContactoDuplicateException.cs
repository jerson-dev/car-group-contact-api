namespace ContactosApi.Domain.Exceptions;

public class ContactoDuplicateException : Exception
{
    public ContactoDuplicateException(string message) : base(message)
    {
    }
}
