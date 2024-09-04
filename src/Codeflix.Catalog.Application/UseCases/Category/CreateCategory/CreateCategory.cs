using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Application.UseCases.Category.Interfaces;
using Codeflix.Catalog.Domain.Exceptions;
using Codeflix.Catalog.Domain.Repository.Interfaces;
using DomainEntity = Codeflix.Catalog.Domain.Entity;

namespace Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

public class CreateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    : ICreateCategory
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<CategoryModelOutput> Handle(
        CreateCategoryInput input,
        CancellationToken cancellationToken
    )
    {
        if (input.Description == null)
        {
            throw new EntityValidationException("Description should not be null");
        }

        var category = new DomainEntity.Category(
            input.Name,
            input.Description ?? string.Empty,
            input.IsActive
        );

        await _categoryRepository.Insert(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return CategoryModelOutput.FromCategory(category);
    }
}
