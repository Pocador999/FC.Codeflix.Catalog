namespace Codeflix.Catalog.Application.UseCases.Category.CreateCategory.Interfaces;

public interface ICreateCategory
{
    public Task<CreateCategoryOutput> Handle(
        CreateCategoryInput input,
        CancellationToken cancellationToken
    );
}
