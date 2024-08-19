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
        Action action = 
            () => new DomainEntity.Category(name!, "Category Description");
        
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
        Action action = 
            () => new DomainEntity.Category("Category Name", description!);
        
        // Act
        var exception = Assert.Throws<EntityValidationException>(action);

        // Assert
        Assert.Equal("Description should not be null", exception.Message);
    } 
}
