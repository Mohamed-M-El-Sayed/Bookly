using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Amenities.Queries.GetCategories;

public record GetAmenityCategoriesQuery : IRequest<Result<IReadOnlyList<AmenityCategoryResponse>>>;
