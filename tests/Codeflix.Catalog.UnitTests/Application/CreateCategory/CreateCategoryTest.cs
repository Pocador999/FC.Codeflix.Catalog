using System;
using Moq;
using Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Domain.Repository.Interfaces;
using Codeflix.Catalog.Application.Interfaces;
using UseCases = Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FluentAssertions;

namespace Codeflix.Catalog.UnitTests.Application.CreateCategory;

public class CreateCategoryTest
{
    [Fact]
    public async void CreateCategory()
    {
        var repositoryMock = new Mock<ICategoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object, 
            unitOfWorkMock.Object
        );

        var input = new UseCases.CreateCategoryInput(
            "Category Name", 
            "Category Description",	
            true
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
            unitOfWork => unitOfWork.Commit(
                It.IsAny<CancellationToken>()
            ), 
            Times.Once
        );

        output.Should().NotBeNull();
        output.Name.Should().Be("Category Name");
        output.Description.Should().Be("Category Description");
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBe(default(Guid));
        output.CreatedAt.Should().NotBe(default(DateTime));
    }
}
