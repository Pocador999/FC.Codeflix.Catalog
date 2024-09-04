using Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace Codeflix.Catalog.UnitTests.Application.UpdateCategory;

public class DataGenerator
{
    public static IEnumerable<object[]> GetCategoryData(int times = 10)
    {
        var fixture = new UpdateCategoryTestFixture();
        for (var i = 0; i < times; i++)
        {
            var category = fixture.GetCategory();
            var input = new UpdateCategoryInput(
                category.Id,
                fixture.GetValidCategoryName(),
                fixture.GetValidCategoryDescription(),
                fixture.GetCategoryBool()
            );
            yield return new object[] { category, input };
        }
    }
}
