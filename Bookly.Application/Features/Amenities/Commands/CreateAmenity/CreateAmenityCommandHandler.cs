using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Amenities.Messages;
using Bookly.Domain.Entities.Amenities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Amenities.Commands.CreateAmenity;

public class CreateAmenityCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateAmenityCommandHandler> logger)
    : IRequestHandler<CreateAmenityCommand, Result<CreateAmenityResponse>>
{
    public async Task<Result<CreateAmenityResponse>> Handle(CreateAmenityCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating amenity with name {NameEn}", request.NameEn);
        var nameEn = request.NameEn.Trim();
        var nameAr = request.NameAr.Trim();
        var repository = unitOfWork.Repository<Amenity>();
        var isDuplicate = await repository.AnyAsync(a => a.NameEn == nameEn || a.NameAr == nameAr, cancellationToken);
        if (isDuplicate)
        {
            logger.LogWarning("Amenity creation failed because the name already exists: {NameEn}", nameEn);
            return Result<CreateAmenityResponse>.Failure(AmenityErrors.NameAlreadyExists());
        }
        var amenity = new Amenity(nameEn, nameAr, request.AmenityCategoryId, request.Scope, request.Icon);
        await repository.AddAsync(amenity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Amenity created successfully with ID {AmenityId}", amenity.Id);
        return Result<CreateAmenityResponse>.Success(amenity.Adapt<CreateAmenityResponse>());
    }
}