using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.AmenityCategories.Messages;
using Bookly.Application.Features.AmenityCategories.Queries.GetCategories;
using Bookly.Application.Features.AmenityCategories.Specifications;
using Bookly.Domain.Entities.Amenities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.AmenityCategories.Queries.GetCategoryById;

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
            return Result<AmenityCategoryResponse>.Failure(AmenityCategoryErrors.CategoryNotFound());
        }
        var response = category.Adapt<AmenityCategoryResponse>();
        return Result<AmenityCategoryResponse>.Success(response);
    }
}
