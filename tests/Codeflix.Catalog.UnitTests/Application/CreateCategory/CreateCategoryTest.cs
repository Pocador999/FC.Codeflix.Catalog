using Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Domain.Exceptions;
using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
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
        var (repositoryMock, unitOfWorkMock, useCase) = _fixture.SetupMocks();
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
        var (repositoryMock, unitOfWorkMock, useCase) = _fixture.SetupMocks();
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
        var (repositoryMock, unitOfWorkMock, useCase) = _fixture.SetupMocks();
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
    [MemberData(nameof(DataGenerator.GetInvalidInputs), parameters: [12], MemberType = typeof(DataGenerator))]
    public async void CreateCategoryFailAggregate(
        CreateCategoryInput input,
        string exceptionMessage
    )
    {
        var (repositoryMock, unitOfWorkMock, useCase) = _fixture.SetupMocks();

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);
    }
}