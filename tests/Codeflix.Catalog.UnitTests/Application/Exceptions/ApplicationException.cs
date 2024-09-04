namespace Codeflix.Catalog.UnitTests.Application.Exceptions;

public abstract class ApplicationException(string? message) : Exception(message)
{
}
