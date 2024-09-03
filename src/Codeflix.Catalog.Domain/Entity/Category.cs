using Codeflix.Catalog.Domain.Exceptions;
using Codeflix.Catalog.Domain.SeedWork;
using Codeflix.Catalog.Domain.Validation;

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
        DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        DomainValidation.MinLenght(Name, nameof(Name), 3);
        DomainValidation.MaxLenght(Name, nameof(Name), 255);
        DomainValidation.NotNull(Description, nameof(Description));
        DomainValidation.MaxLenght(Description, nameof(Description), 10_000);
    }
}
