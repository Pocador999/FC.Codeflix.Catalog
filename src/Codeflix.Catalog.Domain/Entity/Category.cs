namespace Codeflix.Catalog.Domain.Entity;

public class Category
{
    public string Name { get; set; }
    public string Description { get; set; }

    public Category(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
