namespace ContactosApi.Domain.Exceptions;

public class ContactoValidationException : Exception
{
    public ContactoValidationException(string message) : base(message)
    {
    }
}
