using Codeflix.Catalog.UnitTests.Common;
using DomainEntity = Codeflix.Catalog.Domain.Entity;

namespace Codeflix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTestFixture : BaseFixture
{
    public CategoryTestFixture() : base()
    {
    }

    public DomainEntity.Category GetValidCategory()
        => new (
            Faker.Commerce.Categories(1)[0], 
            Faker.Commerce.ProductDescription()
        );
}

[CollectionDefinition(nameof(CategoryTestFixtureCollection))]
public class CategoryTestFixtureCollection
    : ICollectionFixture<CategoryTestFixture>
{   
}
