using System.ComponentModel.DataAnnotations;
using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Domain.Entity;
using FluentAssertions;
using Moq;
using UseCase = Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace Codeflix.Catalog.UnitTests.Application.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixtureCollection))]
public class UpdateCategoryTest(UpdateCategoryTestFixture fixture)
{
    private readonly UpdateCategoryTestFixture _fixture = fixture;

    [Fact]
    public async Task ShouldUpdateCategory()
    {
        var repositoryMock = _fixture.CategoryRepository;
        var unitOfWorkMock = _fixture.UnitOfWork;
        var category = _fixture.GetCategory();

        repositoryMock
            .Setup(x => x.Get(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var input = new UseCase.UpdateCategoryInput(
            category.Id,
            _fixture.GetValidCategoryName(),
            _fixture.GetValidCategoryDescription(),
            !category.IsActive // Invert the current value
        );
        var useCase = new UseCase.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);

        repositoryMock.Verify(
            x => x.Get(category.Id, It.IsAny<CancellationToken>()), 
            Times.Once
        );
        repositoryMock.Verify(
            x => x.Update(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
        unitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()), 
            Times.Once
        );
    }
}
