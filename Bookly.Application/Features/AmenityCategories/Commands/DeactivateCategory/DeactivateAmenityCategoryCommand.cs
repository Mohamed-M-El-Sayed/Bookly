using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.AmenityCategories.Commands.DeactivateCategory;

public record DeactivateAmenityCategoryCommand(int CategoryId) : IRequest<Result>;
