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
    }
}
