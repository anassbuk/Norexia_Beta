namespace Norexia.Core.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base("Not Found Item.")
    {

    }

    public NotFoundException(string? message) : base(message: message)
    {

    }
}
