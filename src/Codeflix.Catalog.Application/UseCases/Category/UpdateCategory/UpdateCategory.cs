using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Application.UseCases.Category.Interfaces;
using Codeflix.Catalog.Domain.Repository.Interfaces;

namespace Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

public class UpdateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    : IUpdateCategory
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CategoryModelOutput> Handle(
        UpdateCategoryInput request,
        CancellationToken cancellationToken
    )
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);
        category.Update(request.Name, request.Description);
        if (request.IsActive != category.IsActive)
            if (request.IsActive)
                category.Activate();
            else
                category.Deactivate();
        await _categoryRepository.Update(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        return CategoryModelOutput.FromCategory(category);
    }
}
