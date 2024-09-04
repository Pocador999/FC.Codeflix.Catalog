using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;

public class DeleteCategoryInput(Guid id) : IRequest<Unit>
{
    public Guid Id { get; } = id;
}
