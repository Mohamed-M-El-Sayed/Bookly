using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Favorites;

namespace Bookly.Application.Features.Favorites.Specifications;

public class GetMyFavoritesSpecification : BaseSpecification<Favorite>
{
    public GetMyFavoritesSpecification(Guid userId)
    {
        Criteria = f => f.UserId == userId;
    }
}

