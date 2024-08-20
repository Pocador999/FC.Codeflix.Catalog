using System.ComponentModel;
using Codeflix.Catalog.Domain.Exceptions;
using DomainEntity = Codeflix.Catalog.Domain.Entity;

namespace Codeflix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTest
{
    [Fact (DisplayName = "InstantiateCategory")]
    public void Instantiate()
    {
        // Arrange
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description"
        };
        // Act
        var datetimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var datetimeAfter = DateTime.Now;

        // Assert
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.True(category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool IsActive)
    {
        // Arrange
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description"
        };
        // Act
        var datetimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(validData.Name, validData.Description, IsActive);
        var datetimeAfter = DateTime.Now;

        // Assert
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.Equal(IsActive, category.IsActive);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        // Arrange
        void action() => new DomainEntity.Category(name!, "Category Description");

        // Act

        var exception = Assert.Throws<EntityValidationException>(action);

        // Assert
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    public void InstantiateErrorWhenDescriptionIsNull(string? description)
    {
        // Arrange
        void action() => new DomainEntity.Category("Category Name", description!);

        // Act

        var exception = Assert.Throws<EntityValidationException>(action);

        // Assert
        Assert.Equal("Description should not be null", exception.Message);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData("12")]
    public void InstantiateErrorWhenNameIsTooShort(string invalidName)
    {
        // Arrange
        void action() => new DomainEntity.Category(invalidName, "Category Ok Description");

        // Act
        var exception = Assert.Throws<EntityValidationException>(action);

        // Assert
        Assert.Equal("Name should have at least 3 characters", exception.Message);	
    }

    [Fact]
    public void InstantiateErrorWhenNameIsTooLong()
    {
        // Arrange
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        void action() => new DomainEntity.Category(invalidName, "Category Ok Description");

        // Act
        var exception = Assert.Throws<EntityValidationException>(action);

        // Assert
        Assert.Equal("Name should have at most 255 characters", exception.Message);
    }

    [Fact]
    public void InstantiateErrorWhenDescriptionIsTooLong()
    {
        // Arrange
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        void action() => new DomainEntity.Category("Category Name", invalidDescription);

        // Act
        var exception = Assert.Throws<EntityValidationException>(action);

        // Assert
        Assert.Equal("Description should have at most 10.000 characters", exception.Message);
    }

    [Fact]
    public void ActivateCategory()
    {
        // Arrange
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description"
        };

        // Act
        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive: false);
        category.Activate();

        // Assert
        Assert.True(category.IsActive);
    }
}
