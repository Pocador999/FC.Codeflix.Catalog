using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.UseCases.Category.Interfaces;
using Codeflix.Catalog.Domain.Repository.Interfaces;
using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;

public class DeleteCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    : IDeleteCategory
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeleteCategoryInput request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);
        await _categoryRepository.Delete(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return Unit.Value;
    }
}
