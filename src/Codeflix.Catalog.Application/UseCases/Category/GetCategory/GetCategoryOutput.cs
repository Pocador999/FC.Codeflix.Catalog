using DomainEntity = Codeflix.Catalog.Domain.Entity;

namespace Codeflix.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategoryOutput(
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

    public static GetCategoryOutput FromCategory(DomainEntity.Category category)
    {
        return new GetCategoryOutput(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive,
            category.CreatedAt
        );
    }
}
