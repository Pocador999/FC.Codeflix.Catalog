using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Domain.Repository.Interfaces;
using FluentAssertions;
using Moq;
using UseCase = Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace Codeflix.Catalog.UnitTests.Application.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixtureCollection))]
public class UpdateCategoryTest(UpdateCategoryTestFixture fixture)
{
    private readonly UpdateCategoryTestFixture _fixture = fixture;

    [Theory(DisplayName = "Update category")]
    [MemberData(
        nameof(DataGenerator.GetCategoryData),
        parameters: 10,
        MemberType = typeof(DataGenerator)
    )]
    public async Task UpdateCategory_ShouldUpdateCategory(
        Category category,
        UseCase.UpdateCategoryInput input
    )
    {
        var repositoryMock = _fixture.CategoryRepository;
        var unitOfWorkMock = _fixture.UnitOfWork;

        repositoryMock
            .Setup(x => x.Get(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var useCase = new UseCase.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);

        repositoryMock.Verify(x => x.Get(category.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(category, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory(DisplayName = "Update category without IsActive")]
    [MemberData(
        nameof(DataGenerator.GetCategoryData),
        parameters: 3,
        MemberType = typeof(DataGenerator)
    )]
    public async Task UpdateCategoryWithoutIsActive_ShouldUpdateCategory(
        Category category,
        UseCase.UpdateCategoryInput input
    )
    {
        var exampleInput = new UseCase.UpdateCategoryInput(
            input.Id,
            input.Name,
            input.Description
        );
        var repositoryMock = _fixture.CategoryRepository;
        var unitOfWorkMock = _fixture.UnitOfWork;

        repositoryMock
            .Setup(x => x.Get(input.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var useCase = new UseCase.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(category.IsActive);

        repositoryMock.Verify(x => x.Get(input.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(category, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory(DisplayName = "Update category with only name")]
    [MemberData(
        nameof(DataGenerator.GetCategoryData),
        parameters: 3,
        MemberType = typeof(DataGenerator)
    )]
    public async Task UpdateCategoryWithOnlyName_ShouldUpdateCategory(
        Category category,
        UseCase.UpdateCategoryInput input
    )
    {
        var exampleInput = new UseCase.UpdateCategoryInput(
            input.Id,
            input.Name
        );
        var repositoryMock = _fixture.CategoryRepository;
        var unitOfWorkMock = _fixture.UnitOfWork;

        repositoryMock
            .Setup(x => x.Get(input.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var useCase = new UseCase.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        CategoryModelOutput output = await useCase.Handle(exampleInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);

        repositoryMock.Verify(x => x.Get(input.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(category, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Throw exception when category not found")]
    public async Task UpdateCategory_ShouldThrowException_WhenNotFound()
    {
        var repositoryMock = _fixture.CategoryRepository;
        var unitOfWorkMock = _fixture.UnitOfWork;
        var input = _fixture.GetUpdateCategoryInput();
        repositoryMock
            .Setup(x => x.Get(input.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Category {input.Id} not found"));

        var useCase = new UseCase.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        repositoryMock.Verify(x => x.Get(input.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
}
