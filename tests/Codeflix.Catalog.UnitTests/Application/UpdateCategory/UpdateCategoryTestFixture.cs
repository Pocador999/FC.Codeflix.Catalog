using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Domain.Repository.Interfaces;
using Codeflix.Catalog.UnitTests.Common;
using Moq;

namespace Codeflix.Catalog.UnitTests.Application.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryTestFixtureCollection))]
public class UpdateCategoryTestFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture> { }

public class UpdateCategoryTestFixture : BaseFixture
{
    public Mock<ICategoryRepository> CategoryRepository => new();
    public Mock<IUnitOfWork> UnitOfWork => new();

    public Category GetCategory()
    {
        return new Category(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetCategoryBool()
        );
    }

    public bool GetCategoryBool() => Faker.Random.Bool();

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

    public UpdateCategoryInput GetUpdateCategoryInput(Guid? id = null) =>
        new(
            id ?? Faker.Random.Guid(),
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetCategoryBool()
        );

    public UpdateCategoryInput GetInvalidShortName()
    {
        var shortName = GetUpdateCategoryInput();
        shortName.Name = shortName.Name[..2];
        return shortName;
    }

    public UpdateCategoryInput GetInvalidLongName()
    {
        var longName = GetUpdateCategoryInput();
        var longNameForCategory = Faker.Commerce.ProductName();
        while (longNameForCategory.Length <= 255)
            longNameForCategory = $"{longNameForCategory} {Faker.Commerce.ProductName()}";
        longName.Name = longNameForCategory;
        return longName;
    }

    public UpdateCategoryInput GetInvalidLongDescription()
    {
        var longDescription = GetUpdateCategoryInput();
        var longDescriptionForCategory = Faker.Commerce.ProductDescription();
        while (longDescriptionForCategory.Length <= 10_000)
            longDescriptionForCategory =
                $"{longDescriptionForCategory} {Faker.Commerce.ProductDescription()}";
        longDescription.Description = longDescriptionForCategory;
        return longDescription;
    }
}
