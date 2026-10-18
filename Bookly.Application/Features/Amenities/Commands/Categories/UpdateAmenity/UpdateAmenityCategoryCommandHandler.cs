using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Amenities.Messages;
using Bookly.Domain.Entities.Amenities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Amenities.Commands.Categories.UpdateAmenity;

public class UpdateAmenityCategoryCommandHandler(
    IUnitOfWork unitOfWork,
    ILogger<UpdateAmenityCategoryCommandHandler> logger)
    : IRequestHandler<UpdateAmenityCategoryCommand, Result>
{
    public async Task<Result> Handle(
        UpdateAmenityCategoryCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Updating amenity category with id {CategoryId}",
            request.CategoryId);
        var repository = unitOfWork.Repository<AmenityCategory>();
        var category = await repository.GetByIdAsync(
            request.CategoryId,
            cancellationToken);

        if (category is null)
            return Result.Failure(AmenityErrors.CategoryNotFound());

        var nameExists = await repository.FindAsync(
            c => c.Id != request.CategoryId &&
                 (c.NameEn == request.NameEn.Trim() ||
                  c.NameAr == request.NameAr.Trim()));

        if (nameExists is not null)
            return Result.Failure(AmenityErrors.CategoryNameAlreadyExists());

        category.Update(request.NameEn, request.NameAr, request.SortOrder);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation(
            "Amenity category with id {CategoryId} updated successfully",
            request.CategoryId);
        return Result.Success();
    }
}