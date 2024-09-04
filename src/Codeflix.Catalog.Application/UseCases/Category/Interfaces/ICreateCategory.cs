using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.Interfaces;

public interface ICreateCategory : IRequestHandler<CreateCategoryInput, CategoryModelOutput>
{
    public new Task<CategoryModelOutput> Handle(
        CreateCategoryInput input,
        CancellationToken cancellationToken
    );
}
