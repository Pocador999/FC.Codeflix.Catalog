using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Domain.Repository.Interfaces;
using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategory(ICategoryRepository categoryRepository) 
    : IRequestHandler<GetCategoryInput, CategoryModelOutput>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<CategoryModelOutput> Handle(
        GetCategoryInput request,
        CancellationToken cancellationToken
    )
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);
        return CategoryModelOutput.FromCategory(category);
    }
}
