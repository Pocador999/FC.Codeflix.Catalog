using Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;

namespace Codeflix.Catalog.UnitTests.Application.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixtureCollection))]
public class UpdateCategoryInputValidatorTest(UpdateCategoryTestFixture fixture)
{
    private readonly UpdateCategoryTestFixture _fixture = fixture;

    [Fact(DisplayName = "Id is required")]
    public void UpdateCategoryInputValidator_WhenIdIsEmpty_ShouldHaveError() // Follow the convention: MethodName_StateUnderTest_ExpectedBehavior
    {
        var input = _fixture.GetUpdateCategoryInput(Guid.Empty);
        var validator = new UpdateCategoryInputValidator();
        var result = validator.Validate(input);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(input.Id));
        result.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }

    [Fact(DisplayName = "Validate input successfully")]
    public void UpdateCategoryInputValidator_WhenInputIsValid_ShouldNotHaveError()
    {
        var input = _fixture.GetUpdateCategoryInput();
        var validator = new UpdateCategoryInputValidator();
        var result = validator.Validate(input);

        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
