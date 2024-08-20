using DomainEntity = Codeflix.Catalog.Domain.Entity;

namespace Codeflix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTestFixture
{
    public DomainEntity.Category GetValidCategory()
        => new ("Category Name", "Category Description");
}

[CollectionDefinition(nameof(CategoryTestFixtureCollection))]
public class CategoryTestFixtureCollection
    : ICollectionFixture<CategoryTestFixture>
{   
}
