using Bookly.Application.Common.Results;
using Bookly.Application.Features.AmenityCategories.Queries.GetCategories;
using MediatR;

namespace Bookly.Application.Features.AmenityCategories.Queries.GetCategoryById;

public record GetAmenityCategoryByIdQuery(int Id) : IRequest<Result<AmenityCategoryResponse>>;
