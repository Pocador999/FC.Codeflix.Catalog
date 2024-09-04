using FluentValidation;

namespace Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;

public class DeleteCategoryInputValidator : AbstractValidator<DeleteCategoryInput>
{
    public DeleteCategoryInputValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");
    }
}
