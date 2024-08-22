using Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Domain.Exceptions;
using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using UseCases = Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FluentAssertions;
using Moq;

namespace Codeflix.Catalog.UnitTests.Application.CreateCategory;

[Collection(nameof(CreateCategoryTestFixtureCollection))]
public class CreateCategoryTest(CreateCategoryTestFixture fixture)
{
    private readonly CreateCategoryTestFixture _fixture = fixture;

    [Fact]
    public async void CreateCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object, 
            unitOfWorkMock.Object
        );
        var input = _fixture.GetInput();
        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<Category>(),
                It.IsAny<CancellationToken>()
            ), 
            Times.Once
        );

        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once 
        );

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBe(default(Guid));
        output.CreatedAt.Should().NotBe(default);
    }

    [Fact]
    public async void CreateCategoryWithOnlyName()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object, 
            unitOfWorkMock.Object
        );
        var input = new CreateCategoryInput(_fixture.GetValidCategoryName());
        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<Category>(),
                It.IsAny<CancellationToken>()
            ), 
            Times.Once
        );
        
        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once 
        );

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(string.Empty);
        output.IsActive.Should().Be(true);
        output.Id.Should().NotBe(default(Guid));
        output.CreatedAt.Should().NotBe(default);
    }

    [Fact]
    public async void CreateCategoryWithOnlyNameAndDescription()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object, 
            unitOfWorkMock.Object
        );
        var input = new CreateCategoryInput(
            _fixture.GetValidCategoryName(),
            _fixture.GetValidCategoryDescription()
        );
        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<Category>(),
                It.IsAny<CancellationToken>()
            ), 
            Times.Once
        );

        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once 
        );

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(true);
        output.Id.Should().NotBe(default(Guid));
        output.CreatedAt.Should().NotBe(default);
    }

    [Theory]
    [MemberData(nameof(GetInvalidInput))]
    public async void CreateCategoryFailAggregate(
        CreateCategoryInput input,
        string exceptionMessage
    )
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);
    }

    public static IEnumerable<object[]> GetInvalidInput()
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidInputList = new List<object[]>();

        // Name can't be less than 3 characters
        var shortName = fixture.GetInput();
            shortName.Name = shortName.Name[..2];
        invalidInputList.Add([shortName, "Name should have at least 3 characters"]);

        // Name can't be more than 255 characters
        var longName = fixture.GetInput();
        var longNameForCategory = fixture.Faker.Commerce.ProductName();
        while (longNameForCategory.Length <= 255)
            longNameForCategory = $"{longNameForCategory} {fixture.Faker.Commerce.ProductName()}";
        longName.Name = longNameForCategory;
        invalidInputList.Add([longName, "Name should have at most 255 characters"]);

        // Description can't be bigger than 10_000 characters
        var longDescription = fixture.GetInput();
        var longDescriptionForCategory = fixture.Faker.Commerce.ProductDescription();
        while (longDescriptionForCategory.Length <= 10_000)
            longDescriptionForCategory = $"{longDescriptionForCategory} {fixture.Faker.Commerce.ProductDescription()}";
        longDescription.Description = longDescriptionForCategory;
        invalidInputList.Add([longDescription, "Description should have at most 10000 characters"]);

        return invalidInputList;
    }
}
