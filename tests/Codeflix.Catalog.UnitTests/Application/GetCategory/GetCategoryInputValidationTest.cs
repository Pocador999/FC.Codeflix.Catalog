using Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using FluentAssertions;

namespace Codeflix.Catalog.UnitTests.Application.GetCategory;

[Collection(nameof(GetCategoryTestFixtureCollection))]
public class GetCategoryInputValidatorTest(GetCategoryTestFixture fixture)
{
    private readonly GetCategoryTestFixture _fixture = fixture;

    [Fact]
    public void Should_InstantiateGetCategoryInput()
    {
        var input = new GetCategoryInput(Guid.NewGuid());
        var validator = new GetCategoryInputValidator();

        var result = validator.Validate(input);

        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Should_ReturnErrorWhenIdIsEmpty()
    {
        var input = new GetCategoryInput(Guid.Empty);
        var validator = new GetCategoryInputValidator();

        var result = validator.Validate(input);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().ContainSingle(x => x.ErrorMessage == "Id is required");
    }
}
