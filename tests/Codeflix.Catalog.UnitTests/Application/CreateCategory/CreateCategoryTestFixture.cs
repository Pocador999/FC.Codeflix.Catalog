using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Domain.Repository.Interfaces;
using Codeflix.Catalog.Domain.SeedWork.Interfaces;
using Codeflix.Catalog.UnitTests.Common;
using Moq;
using Category = Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace Codeflix.Catalog.UnitTests.Application.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixtureCollection))]
public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture> { }

public class CreateCategoryTestFixture : BaseFixture
{
    public static Mock<ICategoryRepository> GetRepositoryMock() => new();

    public static Mock<IUnitOfWork> GetUnitOfWorkMock() => new();

    public string GetValidCategoryName()
    {
        var categoryName = "";
        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255)
            categoryName = categoryName[..255];

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();
        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];

        return categoryDescription;
    }

    public bool GetCategoryBool() => Faker.Random.Bool();

    public CreateCategoryInput GetInput() =>
        new(GetValidCategoryName(), GetValidCategoryDescription(), GetCategoryBool());

    public CreateCategoryInput GetInvalidShortName()
    {
        var shortName = GetInput();
        shortName.Name = shortName.Name[..2];
        return shortName;
    }

    public CreateCategoryInput GetInvalidLongName()
    {
        var longName = GetInput();
        var longNameForCategory = Faker.Commerce.ProductName();
        while (longNameForCategory.Length <= 255)
            longNameForCategory = $"{longNameForCategory} {Faker.Commerce.ProductName()}";
        longName.Name = longNameForCategory;
        return longName;
    }

    public CreateCategoryInput GetInvalidNullDescription()
    {
        var nullDescription = GetInput();
        nullDescription.Description = null!;
        return nullDescription;
    }

    public CreateCategoryInput GetInvalidLongDescription()
    {
        var longDescription = GetInput();
        var longDescriptionForCategory = Faker.Commerce.ProductDescription();
        while (longDescriptionForCategory.Length <= 10_000)
            longDescriptionForCategory =
                $"{longDescriptionForCategory} {Faker.Commerce.ProductDescription()}";
        longDescription.Description = longDescriptionForCategory;
        return longDescription;
    }

    public (Mock<ICategoryRepository>, Mock<IUnitOfWork>, Category.CreateCategory) SetupMocks()
    {
        var repositoryMock = GetRepositoryMock();
        var unitOfWorkMock = GetUnitOfWorkMock();
        var useCase = new Category.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        return (repositoryMock, unitOfWorkMock, useCase);
    }
}
