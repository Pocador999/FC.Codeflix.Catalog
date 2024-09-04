using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategoryInput(Guid id) : IRequest<GetCategoryOutput>
{
    public Guid Id { get; set; } = id;
}
