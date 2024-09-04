using Codeflix.Catalog.Domain.Repository.Interfaces;
using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategory(ICategoryRepository categoryRepository) 
    : IRequestHandler<GetCategoryInput, GetCategoryOutput>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<GetCategoryOutput> Handle(
        GetCategoryInput request,
        CancellationToken cancellationToken
    )
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);
        return GetCategoryOutput.FromCategory(category);
    }
}
