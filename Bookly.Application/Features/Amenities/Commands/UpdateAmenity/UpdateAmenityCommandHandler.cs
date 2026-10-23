using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Amenities.Messages;
using Bookly.Application.Features.Amenities.Specification;
using Bookly.Application.Features.AmenityCategories.Messages;
using Bookly.Domain.Entities.Amenities;
using Bookly.Domain.Entities.Hotels;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Amenities.Commands.UpdateAmenity;

public class UpdateAmenityCommandHandler(IUnitOfWork unitOfWork,
    ILogger<UpdateAmenityCommandHandler> logger) : IRequestHandler<UpdateAmenityCommand, Result>
{
    public async Task<Result> Handle(UpdateAmenityCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating amenity {AmenityId}", request.Id);
        var repository = unitOfWork.Repository<Amenity>();
        var amenity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (amenity is null)
            return Result.Failure(AmenityErrors.NotFound());
        var categoryExists = await unitOfWork.Repository<AmenityCategory>()
            .AnyAsync(c => c.Id == request.AmenityCategoryId, cancellationToken);
        if (!categoryExists)
            return Result.Failure(AmenityCategoryErrors.CategoryNotFound());
        var duplicateExists = await repository.AnyAsync(
                a => a.Id != request.Id &&
                a.CategoryId == request.AmenityCategoryId &&
                (a.NameEn == request.NameEn.Trim() || a.NameAr == request.NameAr.Trim()), cancellationToken);
        if (duplicateExists)
            return Result.Failure(AmenityErrors.NameAlreadyExists());
        if (amenity.ExcludesHotel(request.Scope))
        {
            var hotelsUsingAmenity = await unitOfWork
               .Repository<Hotel>()
               .AnyAsync(
                   new HotelsUsingAmenitySpecification(amenity.Id),
                   cancellationToken);

            if (hotelsUsingAmenity)
                return Result.Failure(AmenityErrors.ScopeInUse());
        }
        if (amenity.ExcludesRoom(request.Scope))
        {
            var roomTypesUsingAmenity = await unitOfWork
                .Repository<RoomType>()
                .AnyAsync(
                    new RoomTypesUsingAmenitySpecification(amenity.Id),
                    cancellationToken);

            if (roomTypesUsingAmenity)
                return Result.Failure(AmenityErrors.ScopeInUse());
        }
        amenity.Update(
            request.NameEn,
            request.NameAr,
            request.AmenityCategoryId,
            request.Scope,
            request.Icon);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();

    }
}
