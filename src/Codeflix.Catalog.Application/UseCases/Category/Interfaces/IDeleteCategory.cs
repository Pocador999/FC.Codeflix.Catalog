using Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.Interfaces;

public interface IDeleteCategory : IRequestHandler<DeleteCategoryInput, Unit> { }
