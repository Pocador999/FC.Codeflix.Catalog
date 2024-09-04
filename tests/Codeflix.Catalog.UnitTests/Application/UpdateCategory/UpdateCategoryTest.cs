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

    [Theory]
    [MemberData(
        nameof(DataGenerator.GetCategoryData),
        parameters: 10,
        MemberType = typeof(DataGenerator)
    )]
    public async Task ShouldUpdateCategory(Category category, UseCase.UpdateCategoryInput input)
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

        repositoryMock.Verify(
            x => x.Get(category.Id, It.IsAny<CancellationToken>()),
            Times.Once
        );
        repositoryMock.Verify(
            x => x.Update(category, It.IsAny<CancellationToken>()),
            Times.Once
        );
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
}
