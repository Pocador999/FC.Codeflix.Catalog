namespace Codeflix.Catalog.Application.UseCases.Category.Common;

public class CategoryModelOutput(
    Guid id,
    string name,
    string description,
    bool isActive,
    DateTime createdAt
)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public bool IsActive { get; set; } = isActive;
    public DateTime CreatedAt { get; set; } = createdAt;

    public static CategoryModelOutput FromCategory(Domain.Entity.Category category)
    {
        return new CategoryModelOutput(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive,
            category.CreatedAt
        );
    }
}
