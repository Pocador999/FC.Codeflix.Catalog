using System.ComponentModel;
using Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
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
        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBe(default(DateTime));
        category.CreatedAt.Should().BeAfter(datetimeBefore).And.BeBefore(datetimeAfter);
        category.IsActive.Should().BeTrue();
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
        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBe(default(DateTime));
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
        Action action = 
            () => new DomainEntity.Category(name!, "Category Ok Description");

        // Act & Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Theory]
    [InlineData(null)]
    public void InstantiateErrorWhenDescriptionIsNull(string? description)
    {
        // Arrange
        Action action = 
            () => new DomainEntity.Category("Category Name", description!);
        
        // Act & Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should not be null");
    }

    [Theory]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData("12")]
    public void InstantiateErrorWhenNameIsTooShort(string invalidName)
    {
        // Arrange
        Action action = 
            () => new DomainEntity.Category(invalidName, "Category Ok Description");

        // Act & Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should have at least 3 characters");
    }

    [Fact]
    public void InstantiateErrorWhenNameIsTooLong()
    {
        // Arrange
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        Action action = 
            () => new DomainEntity.Category(invalidName, "Category Ok Description");

        // Act & Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should have at most 255 characters");
    }

    [Fact]
    public void InstantiateErrorWhenDescriptionIsTooLong()
    {
        // Arrange
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        Action action = 
            () => new DomainEntity.Category("Category Name", invalidDescription);

        // Act & Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should have at most 10.000 characters");
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
        category.IsActive.Should().BeTrue();
    }

    [Fact]
    public void DeactivateCategory()
    {
        // Arrange
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description"
        };

        // Act
        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive: true);
        category.Deactivate();

        // Assert
        category.IsActive.Should().BeFalse();
    }

    [Fact]
    public void UpdateCategory()
    {
        // Arrange
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var newValues = new { Name = "New Category Name", Description = "New Category Description" };

        // Act
        category.Update(newValues.Name, newValues.Description);

        // Assert
        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(newValues.Description);
    }

    [Fact]
    public void UpdateOnlyName()
    {
        // Arrange
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var newValues = new { Name = "New Category Name" };

        // Act
        category.Update(newValues.Name);

        // Assert
        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be("Category Description");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        // Arrange
        var category = new DomainEntity.Category("Category Name", "Category Description");
        Action action = 
            () => category.Update(name!);

        // Act & Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Theory]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData("12")]
    public void UpdateErrorWhenNameIsTooShort(string invalidName)
    {
        // Arrange
        var category = new DomainEntity.Category("Category Name", "Category Description");
        Action action = 
            () => category.Update(invalidName);

        // Act & Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should have at least 3 characters");
    }

    [Fact]
    public void UpdateErrorWhenNameIsTooLong()
    {
        // Arrange
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        Action action = 
            () => category.Update(invalidName);
        
        // Act & Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should have at most 255 characters");
    }

    [Fact]
    public void UpdateErrorWhenDescriptionIsTooLong()
    {
        // Arrange
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        Action action = 
            () => category.Update("Category Name", invalidDescription);

        // Act & Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should have at most 10.000 characters");
    }
}
