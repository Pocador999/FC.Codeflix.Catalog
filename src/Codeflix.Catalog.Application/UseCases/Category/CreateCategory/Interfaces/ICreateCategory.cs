using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.CreateCategory.Interfaces;

public interface ICreateCategory : IRequestHandler<CreateCategoryInput, CreateCategoryOutput>
{
    public new Task<CreateCategoryOutput> Handle(
        CreateCategoryInput input,
        CancellationToken cancellationToken
    );
}
