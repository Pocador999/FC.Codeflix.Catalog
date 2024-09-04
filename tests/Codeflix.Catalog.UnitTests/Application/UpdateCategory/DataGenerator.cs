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
            var input = fixture.GetUpdateCategoryInput(category.Id);
            yield return new object[] { category, input };
        }
    }

    public static IEnumerable<object[]> GetInvalidCategoryData(int reps = 12)
    {
        var fixture = new UpdateCategoryTestFixture();
        var invalidInputsList = new List<object[]>();

        var invalidCases = new Dictionary<int, Func<(UpdateCategoryInput, string)>>()
        {
            { 0, () => (fixture.GetInvalidShortName(), "Name should have at least 3 characters") },
            { 1, () => (fixture.GetInvalidLongName(), "Name should have at most 255 characters") },
            { 2, () => (fixture.GetInvalidLongDescription(), "Description should have at most 10000 characters") }
        };

        for (int i = 0; i < reps; i++)
        {
            var caseIndex = i % invalidCases.Count;
            var (input, errorMessage) = invalidCases[caseIndex]();
            invalidInputsList.Add([input, errorMessage]);
        }

        return invalidInputsList;
    }
}
