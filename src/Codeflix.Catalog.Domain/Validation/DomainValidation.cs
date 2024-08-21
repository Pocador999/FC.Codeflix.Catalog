using Codeflix.Catalog.Domain.Exceptions;

namespace Codeflix.Catalog.Domain.Validation;

public class DomainValidation
{
    public static void NotNull(object? target, string fieldName)
    {
        if (target == null)
            throw new EntityValidationException($"{fieldName} should not be null");
    }

    public static void NotNullOrEmpty(string? target, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(target))
            throw new EntityValidationException($"{fieldName} should not be empty or null");
    }

    public static void MinLenght(string target, string fieldName, int minLenght)
    {
        if (target.Length < minLenght)
            throw new EntityValidationException($"{fieldName} should have at least {minLenght} characters");
    }

    public static void MaxLenght(string target, string fieldName, int maxLenght)
    {
        if (target.Length > maxLenght)
            throw new EntityValidationException($"{fieldName} should have at most {maxLenght} characters");
    }
}
