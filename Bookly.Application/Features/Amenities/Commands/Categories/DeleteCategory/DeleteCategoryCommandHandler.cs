using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Amenities.Messages;
using Bookly.Domain.Entities.Amenities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Amenities.Commands.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler(IUnitOfWork unitOfWork,
    ILogger<DeleteCategoryCommandHandler> logger) : IRequestHandler<DeleteCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting amenity category with ID: {CategoryId}", request.Id);
        var category = await unitOfWork.Repository<AmenityCategory>().GetByIdAsync(request.Id);
        if (category is null)
            return Result.Failure(AmenityErrors.CategoryNotFound());
        var hasAmenities = await unitOfWork.Repository<Amenity>().AnyAsync(a => a.CategoryId == request.Id);
        if (hasAmenities)
        {
            // Cannot delete the category if it has associated amenities
            logger.LogWarning("Delete rejected: category {CategoryId} still contains amenities", category.Id);
            return Result.Failure(AmenityErrors.CategoryHasAmenities());
        }
        unitOfWork.Repository<AmenityCategory>().Remove(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Amenity category {CategoryId} deleted", category.Id);
        return Result.Success();
    }
}
