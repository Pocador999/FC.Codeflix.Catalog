using Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategoryInput(Guid id) : IRequest<CategoryModelOutput>
{
    public Guid Id { get; set; } = id;
}
