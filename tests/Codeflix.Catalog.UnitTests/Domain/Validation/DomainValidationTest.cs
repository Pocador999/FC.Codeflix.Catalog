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

    [Theory]
    [MemberData(nameof(GetMinLengthFailData), parameters: 10)]
    public void MinLengthFail(string target, int minLength)
    {
        Action action = 
            () => DomainValidation.MinLenght(target, nameof(target), minLength);
    
        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{nameof(target)} should have at least {minLength} characters");
    }

    [Theory]
    [MemberData(nameof(GetMinLenghtOkData), parameters: 10)]
    public void MinLenghtOk(string target, int minLength)
    {
        Action action = 
            () => DomainValidation.MinLenght(target, nameof(target), minLength);
        
        action.Should().NotThrow();
    }

    [Theory]
    [MemberData(nameof(GetMaxLengthFailData), parameters: 10)]
    public void MaxLenghtFail(string target, int maxLenght)
    {
        Action action = 
            () => DomainValidation.MaxLenght(target, nameof(target), maxLenght);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{nameof(target)} should have at most {maxLenght} characters");
    }    

    [Theory]
    [MemberData(nameof(GetMaxLenghtOkData), parameters: 10)]
    public void MaxLenghtOk(string target, int maxLenght)
    {
        Action action = 
            () => DomainValidation.MaxLenght(target, nameof(target), maxLenght);

        action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GetMinLengthFailData(int numTests = 3)
    {
        yield return new object[] { "132456", 10 }; 
        var faker = new Faker();

        for (int i = 0; i < (numTests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length + faker.Random.Int(1, 20);

            yield return new object[] { example, minLength };
        }
    }

    public static IEnumerable<object[]> GetMinLenghtOkData(int numTests = 3)
    {
        yield return new object[] { "132456", 6 }; 
        var faker = new Faker();

        for (int i = 0; i < (numTests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length - faker.Random.Int(1, 5);

            yield return new object[] { example, minLength };
        }
    }

    public static IEnumerable<object[]> GetMaxLengthFailData(int numTests = 3)
    {
        yield return new object[] { "132456", 5 }; 
        var faker = new Faker();

        for (int i = 0; i < (numTests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length - faker.Random.Int(1, 5);

            yield return new object[] { example, maxLength };
        }
    }

    public static IEnumerable<object[]> GetMaxLenghtOkData(int numTests = 3)
    {
        yield return new object[] { "132456", 6 }; 
        var faker = new Faker();

        for (int i = 0; i < (numTests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length + faker.Random.Int(0, 5);

            yield return new object[] { example, maxLength };
        }
    }
}
