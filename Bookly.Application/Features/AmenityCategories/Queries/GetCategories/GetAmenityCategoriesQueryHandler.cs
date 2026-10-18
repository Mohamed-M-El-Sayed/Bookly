using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Application.Features.AmenityCategories.Specifications;
using Bookly.Domain.Entities.Amenities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.AmenityCategories.Queries.GetCategories;

public class GetAmenityCategoriesQueryHandler(IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ILogger<GetAmenityCategoriesQueryHandler> logger) : IRequestHandler<GetAmenityCategoriesQuery, Result<IReadOnlyList<AmenityCategoryResponse>>>
{
    public async Task<Result<IReadOnlyList<AmenityCategoryResponse>>> Handle(GetAmenityCategoriesQuery request, CancellationToken cancellationToken)
    {
        var isAdmin = currentUserService.IsAdmin;
        logger.LogInformation(
            "Retrieving amenity categories. Include inactive: {IncludeInactive}",
            isAdmin);
        var categories = await unitOfWork.Repository<AmenityCategory>()
            .GetAllWithSpecAsync(new AmenityCategoriesSpecification(isAdmin), cancellationToken);
        var result = categories.Adapt<IReadOnlyList<AmenityCategoryResponse>>();
        return Result<IReadOnlyList<AmenityCategoryResponse>>.Success(result);
    }
}
