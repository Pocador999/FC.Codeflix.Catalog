using Codeflix.Catalog.Domain.Exceptions;

namespace Codeflix.Catalog.Domain.Entity;

public class Category
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public Guid Id { get; set; }

    public Category(string name, string description, bool isActive = true)
    {
        Name = name;
        Description = description;
        CreatedAt = DateTime.Now;
        IsActive = isActive;
        Id = Guid.NewGuid();

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

    public void Activate()
    {
        IsActive = true;
        Validate();
    }
}
