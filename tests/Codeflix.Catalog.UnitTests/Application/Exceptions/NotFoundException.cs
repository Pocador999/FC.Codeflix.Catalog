namespace Codeflix.Catalog.UnitTests.Application.Exceptions;

public class NotFoundException(string? message) : ApplicationException(message)
{
}
