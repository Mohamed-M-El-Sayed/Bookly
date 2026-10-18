using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Amenities.Commands.Categories.DeleteCategory;

public record DeleteCategoryCommand(int Id) : IRequest<Result>;

