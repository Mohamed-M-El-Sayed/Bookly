using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.AmenityCategories.Messages;
using Bookly.Domain.Entities.Amenities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.AmenityCategories.Commands.CreateCategory
{
    public class CreateAmenityCategoryCommandHandler(IUnitOfWork unitOfWork,
        ILogger<CreateAmenityCategoryCommandHandler> logger) : IRequestHandler<CreateAmenityCategoryCommand, Result<CreateAmenityCategoryResponse>>
    {
        public async Task<Result<CreateAmenityCategoryResponse>> Handle(CreateAmenityCategoryCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating amenity category");
            var existingCategory = await unitOfWork.Repository<AmenityCategory>().FindAsync(c =>
            (c.NameEn == request.NameEn.Trim() || c.NameAr == request.NameAr.Trim()));
            if (existingCategory is not null)
                return Result<CreateAmenityCategoryResponse>.Failure(AmenityCategoryErrors.CategoryNameAlreadyExists());
            var category = new AmenityCategory(request.NameEn, request.NameEn, request.SortOrder);
            await unitOfWork.Repository<AmenityCategory>().AddAsync(category, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Amenity category created with id {CategoryId}", category.Id);
            return Result<CreateAmenityCategoryResponse>.Success(new(category.Id, request.NameEn, request.NameAr));
        }
    }
}
