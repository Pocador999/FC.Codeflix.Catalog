﻿using DomainEntity = Codeflix.Catalog.Domain.Entity; 

namespace Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

public class CreateCategoryOutput(
    Guid id, 
    string name, 
    string description,
    DateTime createdAt,
    bool isActive = true
)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description ?? string.Empty;
    public bool IsActive { get; set; } = isActive;
    public DateTime CreatedAt { get; set; } = createdAt;

    public static CreateCategoryOutput FromCategory(DomainEntity.Category category)
    {
        return new CreateCategoryOutput(
            category.Id,
            category.Name,
            category.Description,
            category.CreatedAt,
            category.IsActive
        );
    }
}
