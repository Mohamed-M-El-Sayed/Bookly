using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.AmenityCategories.Messages;
using Bookly.Domain.Entities.Amenities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.AmenityCategories.Commands.ActivateCategory;

public class ActivateAmenityCategoryCommandHandler(IUnitOfWork unitOfWork,
    ILogger<ActivateAmenityCategoryCommandHandler> logger) : IRequestHandler<ActivateAmenityCategoryCommand, Result>
{
    public async Task<Result> Handle(ActivateAmenityCategoryCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Activating");
        var category = await unitOfWork.Repository<AmenityCategory>()
            .GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
            return Result.Failure(AmenityCategoryErrors.CategoryNotFound());
        if (category.IsActive)
            return Result.Success();
        category.Activate();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Category with id is activited");
        return Result.Success();
    }
}
