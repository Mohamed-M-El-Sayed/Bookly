using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.AmenityCategories.Messages;
using Bookly.Domain.Entities.Amenities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.AmenityCategories.Commands.DeactivateCategory;

public class DeactivateAmenityCategoryCommandHandler(IUnitOfWork unitOfWork,
    ILogger<DeactivateAmenityCategoryCommandHandler> logger) : IRequestHandler<DeactivateAmenityCategoryCommand, Result>
{
    public async Task<Result> Handle(DeactivateAmenityCategoryCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deactivating amenity category {CategoryId}", request.CategoryId);
        var category = await unitOfWork.Repository<AmenityCategory>()
            .GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
            return Result.Failure(AmenityCategoryErrors.CategoryNotFound());
        if (!category.IsActive)
            return Result.Success();
        var hasActiveAmenities = await unitOfWork.Repository<Amenity>()
            .AnyAsync(a => a.CategoryId == category.Id && a.IsActive, cancellationToken);
        if (hasActiveAmenities)
            return Result.Failure(AmenityCategoryErrors.CategoryInUse());
        category.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Amenity category {CategoryId} deactivated", category.Id);
        return Result.Success();
    }
}
