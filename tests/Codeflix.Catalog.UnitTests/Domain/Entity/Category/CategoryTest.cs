using Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using DomainEntity = Codeflix.Catalog.Domain.Entity;

namespace Codeflix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixtureCollection))]
public class CategoryTest(CategoryTestFixture categoryTestFixture)
{
    private readonly CategoryTestFixture _categoryTestFixture = categoryTestFixture;

    [Fact (DisplayName = "InstantiateCategory")]
    public void Instantiate()
    {
        // Arrange
        var validCategory = _categoryTestFixture.GetValidCategory();

        // Act
        var datetimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        // Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBe(default);
        category.CreatedAt.Should().BeAfter(datetimeBefore).And.BeBefore(datetimeAfter);
        category.IsActive.Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool IsActive)
    {
        // Arrange
        var validCategory = _categoryTestFixture.GetValidCategory();

        // Act
        var datetimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, IsActive);
        var datetimeAfter = DateTime.Now.AddSeconds(1);


        // Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBe(default);
        category.CreatedAt.Should().BeAfter(datetimeBefore).And.BeBefore(datetimeAfter);
        category.IsActive.Should().Be(IsActive);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        // Arrange
        var validCategory = _categoryTestFixture.GetValidCategory();

        // Act
        Action action = 
            () => new DomainEntity.Category(name!, validCategory.Description);

        // Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Theory]
    [InlineData(null)]
    public void InstantiateErrorWhenDescriptionIsNull(string? description)
    {
        // Arrange
        var validCategory = _categoryTestFixture.GetValidCategory();

        // Act
        Action action = 
            () => new DomainEntity.Category(validCategory.Name, description!);
        
        // Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should not be null");
    }

    [Theory]
    [MemberData(nameof(GetInvalidShortName), parameters: 10)]
    public void InstantiateErrorWhenNameIsTooShort(string invalidName)
    {
        // Arrange
        var validCategory = _categoryTestFixture.GetValidCategory();

        // Act
        Action action = 
            () => new DomainEntity.Category(invalidName, validCategory.Description);

        // Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should have at least 3 characters");
    }

    public static IEnumerable<object[]> GetInvalidShortName(int numTests = 6)
    {
        var fixture = new CategoryTestFixture();

        for(int i = 0; i < numTests; i++)
        {
            var isOdd = i % 2 == 1;
            yield return new object[] { 
                fixture.GetValidCategoryName()[..(isOdd ? 1 : 2)]
            };
        }
    }

    [Fact]
    public void InstantiateErrorWhenNameIsTooLong()
    {
        // Arrange
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidName = string.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

        // Act
        Action action = 
            () => new DomainEntity.Category(invalidName, validCategory.Description);

        // Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should have at most 255 characters");
    }

    [Fact]
    public void InstantiateErrorWhenDescriptionIsTooLong()
    {
        // Arrange
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidDescription = string.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());

        // Act
        Action action = 
            () => new DomainEntity.Category(validCategory.Name, invalidDescription);

        // Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should have at most 10000 characters");
    }

    [Fact]
    public void ActivateCategory()
    {
        // Arrange
        var validCategory = _categoryTestFixture.GetValidCategory();

        // Act
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive: false);
        category.Activate();

        // Assert
        category.IsActive.Should().BeTrue();
    }

    [Fact]
    public void DeactivateCategory()
    {
        // Arrange
        var validCategory = _categoryTestFixture.GetValidCategory();

        // Act
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive: true);
        category.Deactivate();

        // Assert
        category.IsActive.Should().BeFalse();
    }

    [Fact]
    public void UpdateCategory()
    {
        // Arrange
        var category = _categoryTestFixture.GetValidCategory();
        var categoryWithNewValues = _categoryTestFixture.GetValidCategory();

        // Act
        category.Update(categoryWithNewValues.Name, categoryWithNewValues.Description);

        // Assert
        category.Name.Should().Be(categoryWithNewValues.Name);
        category.Description.Should().Be(categoryWithNewValues.Description);
    }

    [Fact]
    public void UpdateOnlyName()
    {
        // Arrange
        var category = _categoryTestFixture.GetValidCategory();
        var newName = _categoryTestFixture.GetValidCategoryName();
        var description = category.Description;

        // Act
        category.Update(newName);

        // Assert
        category.Name.Should().Be(newName);
        category.Description.Should().Be(description);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        // Arrange
        var category = _categoryTestFixture.GetValidCategory();

        // Act
        Action action = 
            () => category.Update(name!);

        // Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Theory]
    [MemberData(nameof(GetInvalidShortName), parameters: 6)]
    public void UpdateErrorWhenNameIsTooShort(string invalidName)
    {
        // Arrange
        var category = _categoryTestFixture.GetValidCategory();

        // Act
        Action action = 
            () => category.Update(invalidName);

        // Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should have at least 3 characters");
    }

    [Fact]
    public void UpdateErrorWhenNameIsTooLong()
    {
        // Arrange
        var category = _categoryTestFixture.GetValidCategory();
        var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);

        // Act
        Action action = 
            () => category.Update(invalidName);
        
        // Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should have at most 255 characters");
    }

    [Fact]
    public void UpdateErrorWhenDescriptionIsTooLong()
    {
        // Arrange
        var category = _categoryTestFixture.GetValidCategory();
        var invalidDescription = _categoryTestFixture.Faker.Commerce.ProductDescription();
        while (invalidDescription.Length <= 10_000)
            invalidDescription = $"{invalidDescription} {_categoryTestFixture.Faker.Commerce.ProductDescription()}";

        // Act
        Action action = 
            () => category.Update("Category Name", invalidDescription);

        // Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should have at most 10000 characters");
    }

    [Fact]
    public void TestInvalidNullDescription()
    {
        // Arrange
        var category = _categoryTestFixture.GetValidCategory();

        // Act
        Action action = 
            () => category.Update("Name", null);
        category.Description = null!;

        // Assert
        category.Description.Should().Be(null);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should not be null");
    }
}
