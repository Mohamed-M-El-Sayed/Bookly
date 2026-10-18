using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Amenities.Messages;
using Bookly.Application.Features.Amenities.Queries.GetCategories;
using Bookly.Application.Features.Amenities.Specifications;
using Bookly.Domain.Entities.Amenities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Amenities.Queries.GetCategoryById;

public class GetAmenityCategoryByIdQueryHandler(IUnitOfWork unitOfWork,
    ILogger<GetAmenityCategoryByIdQueryHandler> logger) : IRequestHandler<GetAmenityCategoryByIdQuery, Result<AmenityCategoryResponse>>
{
    public async Task<Result<AmenityCategoryResponse>> Handle(GetAmenityCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting amentiy category wiht Id: {CategoryId}", request.Id);
        var category = await unitOfWork.Repository<AmenityCategory>()
            .GetEntityWithSpecAsync(new AmenityCategoryByIdSpecification(request.Id));
        if (category is null)
        {
            logger.LogWarning("Amenity category not found");
            return Result<AmenityCategoryResponse>.Failure(AmenityErrors.CategoryNotFound());
        }
        var response = category.Adapt<AmenityCategoryResponse>();
        return Result<AmenityCategoryResponse>.Success(response);
    }
}
