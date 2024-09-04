using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.Interfaces;

public interface IGetCategory : IRequestHandler<GetCategoryInput, CategoryModelOutput>
{
    public new Task<CategoryModelOutput> Handle(
        GetCategoryInput request,
        CancellationToken cancellationToken
    );
}
