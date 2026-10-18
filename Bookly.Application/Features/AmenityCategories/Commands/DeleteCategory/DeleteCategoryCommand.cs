using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.AmenityCategories.Commands.DeleteCategory;

public record DeleteCategoryCommand(int Id) : IRequest<Result>;

