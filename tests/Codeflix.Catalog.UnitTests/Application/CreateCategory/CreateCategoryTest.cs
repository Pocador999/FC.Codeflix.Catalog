using Codeflix.Catalog.Domain.Entity;
using UseCases = Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FluentAssertions;
using Moq;

namespace Codeflix.Catalog.UnitTests.Application.CreateCategory;

[Collection(nameof(CreateCategoryTestFixtureCollection))]
public class CreateCategoryTest
{
    private readonly CreateCategoryTestFixture _fixture;

    public CreateCategoryTest(CreateCategoryTestFixture fixture)
        => _fixture = fixture;

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
        output.CreatedAt.Should().NotBe(default(DateTime));
    }
}
