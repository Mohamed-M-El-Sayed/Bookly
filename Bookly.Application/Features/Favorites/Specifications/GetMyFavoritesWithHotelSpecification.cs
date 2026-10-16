using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Favorites;

namespace Bookly.Application.Features.Favorites.Specifications;

public class GetMyFavoritesWithHotelSpecification : BaseSpecification<Favorite>
{
    public GetMyFavoritesWithHotelSpecification(Guid userId)
    {
        Criteria = f => f.UserId == userId;

        AddInclude(f => f.Hotel);
        AddInclude(f => f.Hotel!.Images.Where(i => i.IsPrimary));
    }
}