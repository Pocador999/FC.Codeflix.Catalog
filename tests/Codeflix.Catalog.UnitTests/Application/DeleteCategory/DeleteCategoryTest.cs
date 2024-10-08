using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using UseCase = Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;

namespace Codeflix.Catalog.UnitTests.Application.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixtureCollection))]
public class DeleteCategoryTest(DeleteCategoryTestFixture fixture)
{
    private readonly DeleteCategoryTestFixture _fixture = fixture;

    [Fact]
    public async Task ShouldDeleteCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var validatorMock = new Mock<IValidator<DeleteCategoryInput>>();
        var category = _fixture.GetValidCategory();

        repositoryMock
            .Setup(x => x.Get(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        validatorMock
            .Setup(x =>
                x.ValidateAsync(It.IsAny<DeleteCategoryInput>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(new ValidationResult());

        var input = new DeleteCategoryInput(category.Id);
        var useCase = new UseCase.DeleteCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object,
            validatorMock.Object
        );

        await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(x => x.Get(category.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Delete(category, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.VerifyNoOtherCalls();
        unitOfWorkMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldNotDeleteCategoryWhenCategoryNotFound()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var validatorMock = new Mock<IValidator<DeleteCategoryInput>>();
        var exampleGuid = Guid.NewGuid();

        repositoryMock
            .Setup(x => x.Get(exampleGuid, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Category {exampleGuid} not found."));

        validatorMock
            .Setup(x =>
                x.ValidateAsync(It.IsAny<DeleteCategoryInput>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(new ValidationResult());

        var input = new DeleteCategoryInput(exampleGuid);
        var useCase = new UseCase.DeleteCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object,
            validatorMock.Object
        );

        var task = async () => await useCase.Handle(input, CancellationToken.None);
        await task.Should().ThrowAsync<NotFoundException>();

        repositoryMock.Verify(x => x.Get(exampleGuid, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.VerifyNoOtherCalls();
        unitOfWorkMock.VerifyNoOtherCalls();
    }
}
