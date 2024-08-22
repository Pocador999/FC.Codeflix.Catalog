using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Domain.Repository.Interfaces;
using Codeflix.Catalog.UnitTests.Common;
using Moq;

namespace Codeflix.Catalog.UnitTests.Application.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixtureCollection))]
public class CreateCategoryTestFixtureCollection
    : ICollectionFixture<CreateCategoryTestFixture>
{
}

public class CreateCategoryTestFixture : BaseFixture
{
    public string GetValidCategoryName()
    {
        var categoryName = "";
        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];

        if(categoryName.Length > 255)
            categoryName = categoryName[..255];
            
        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();
        if(categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];
            
        return categoryDescription;
    }

    public bool GetCategoryBool()
        => Faker.Random.Bool();

    public CreateCategoryInput GetInput()
        => new (
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetCategoryBool()
        );
    public Mock<ICategoryRepository> GetRepositoryMock()
        => new();
    public Mock<IUnitOfWork> GetUnitOfWorkMock()
        => new();

    // public Mock<ICategoryRepository> GetRepositoryMock()
    //     => new Mock<ICategoryRepository>();
    // public Mock<IUnitOfWork> GetUnitOfWorkMock()
    //     => new Mock<IUnitOfWork>();
}
