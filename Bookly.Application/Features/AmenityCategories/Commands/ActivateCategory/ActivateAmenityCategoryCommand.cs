using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.AmenityCategories.Commands.ActivateCategory;

public record ActivateAmenityCategoryCommand(int CategoryId) : IRequest<Result>;
