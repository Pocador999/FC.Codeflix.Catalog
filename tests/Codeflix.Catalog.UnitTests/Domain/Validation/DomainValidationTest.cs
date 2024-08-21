using Codeflix.Catalog.Domain.Validation;
using Bogus;
using FluentAssertions;
using Codeflix.Catalog.Domain.Exceptions;

namespace Codeflix.Catalog.UnitTests.Domain.Validation;

public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker();
    
    [Fact]
    public void NotNullOk()
    {
        var value = Faker.Commerce.ProductName();

        Action action = 
            () => DomainValidation.NotNull(value, nameof(value));

        action.Should().NotThrow();
    }

    [Fact]
    public void NotNullFail()
    {
        string? value = null;

        Action action = 
            () => DomainValidation.NotNull(value, nameof(value));

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{nameof(value)} should not be null");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void NotNullOrEmptyFailWhenEmpty(string? target)
    {
        Action action = 
            () => DomainValidation.NotNullOrEmpty(target, nameof(target));

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{nameof(target)} should not be empty or null");
    }

    [Fact]
    public void NotNullOrEmptyOk()
    {
        var target = Faker.Commerce.ProductName();

        Action action = 
            () => DomainValidation.NotNullOrEmpty(target, nameof(target));

        action.Should().NotThrow();
    }
}
