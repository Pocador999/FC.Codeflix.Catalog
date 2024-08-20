using Codeflix.Catalog.Domain.Exceptions;
using Codeflix.Catalog.Domain.SeedWork;

namespace Codeflix.Catalog.Domain.Entity;

public class Category : AggregateRoot
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public Category(string name, string description, bool isActive = true) 
        : base()
    {
        Name = name;
        Description = description;
        CreatedAt = DateTime.Now;
        IsActive = isActive;

        Validate();
    }

    public void Activate()
    {
        IsActive = true;
        Validate();
    }

    public void Deactivate()
    {
        IsActive = false;
        Validate();
    }

    public void Update(string name, string? description = null)
    {
        Name = name;
        Description = description ?? Description;
        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new EntityValidationException($"{nameof(Name)} should not be empty or null");

        if (Description == null)
            throw new EntityValidationException($"{nameof(Description)} should not be null");

        if (Name.Length < 3)
            throw new EntityValidationException($"{nameof(Name)} should have at least 3 characters");
        
        if (Name.Length > 255)
            throw new EntityValidationException($"{nameof(Name)} should have at most 255 characters");

        if (Description.Length > 10_000)
            throw new EntityValidationException($"{nameof(Description)} should have at most 10.000 characters");
    }
}
