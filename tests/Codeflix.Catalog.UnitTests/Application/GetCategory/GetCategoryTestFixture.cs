using Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Domain.Repository.Interfaces;
using Codeflix.Catalog.UnitTests.Common;
using Moq;

namespace Codeflix.Catalog.UnitTests.Application.GetCategory;

[CollectionDefinition(nameof(GetCategoryTestFixtureCollection))]
public class GetCategoryTestFixtureCollection : ICollectionFixture<GetCategoryTestFixture> { }

public class GetCategoryTestFixture : BaseFixture
{
    public Category GetCategory() => new(GetValidCategoryName(), GetValidCategoryDescription());

    public Mock<ICategoryRepository> GetRepositoryMock() => new();

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
}
