using Bookly.Application.Common.Results;
using Bookly.Application.Features.Amenities.Queries.GetCategories;
using MediatR;

namespace Bookly.Application.Features.Amenities.Queries.GetCategoryById;

public record GetAmenityCategoryByIdQuery(int Id) : IRequest<Result<AmenityCategoryResponse>>;
