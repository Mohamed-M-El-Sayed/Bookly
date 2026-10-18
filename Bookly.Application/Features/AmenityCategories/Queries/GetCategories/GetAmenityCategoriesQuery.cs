using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.AmenityCategories.Queries.GetCategories;

public record GetAmenityCategoriesQuery : IRequest<Result<IReadOnlyList<AmenityCategoryResponse>>>;
