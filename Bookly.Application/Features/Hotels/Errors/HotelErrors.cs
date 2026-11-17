using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Hotels.Errors;

public static class HotelErrors
{
    public static Error NotFound() =>
        ErrorFactory.NotFound("Hotel.NotFound", "Hotel not found.");
    public static Error NotManager() =>
           ErrorFactory.Forbidden("Hotel.NotManager", "You are not authorized to manage this hotel.");
    public static Error ManagerAlreadyAssigned() =>
        ErrorFactory.Conflict("Hotel.ManagerAlreadyAssigned", "User is already assigned to a hotel.");
    public static Error ManagerNotAssigned() =>
        ErrorFactory.Conflict("Hotel.ManagerNotAssigned", "This user is not assigned as a manager of this hotel.");

}