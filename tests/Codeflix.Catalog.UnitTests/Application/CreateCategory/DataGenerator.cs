using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace Codeflix.Catalog.UnitTests.Application.CreateCategory;

public class DataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int reps = 12)
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidInputsList = new List<object[]>();

        var invalidCases = new Dictionary<int, Func<(CreateCategoryInput, string)>>()
        {
            { 0, () => (fixture.GetInvalidShortName(), "Name should have at least 3 characters") },
            { 1, () => (fixture.GetInvalidLongName(), "Name should have at most 255 characters") },
            { 2, () => (fixture.GetInvalidNullDescription(), "Description should not be null") },
            { 3, () => (fixture.GetInvalidLongDescription(), "Description should have at most 10000 characters") }
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