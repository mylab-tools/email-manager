namespace MyLab.EmailManager.Domain.Exceptions;

public class DomainValidationException : DomainException
{
    public DomainValidationException(string message) : base(message)
    {

    }

    public DomainValidationException(string message, Exception inner) : base(message, inner)
    {

    }
}