using Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

public class UpdateCategoryInput(Guid id, string name, string? description = null, bool? isActive = null)
    : IRequest<CategoryModelOutput>
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string? Description { get; set; } = description ?? null;
    public bool IsActive { get; set; } = isActive ?? false;
}
