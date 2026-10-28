using Bookly.Application.Common.Models;
using Bookly.Application.Common.Results;
using Bookly.Application.Features.Bookings.Shared;
using Bookly.Domain.Enums;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.Bookings.Queries.GetHotelBookings;

public class GetHotelBookingsQuery : IRequest<Result<PagedResult<BookingResponse>>>
{
    [JsonIgnore]
    public int HotelId { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public BookingStatus? Status { get; set; }
    public DateOnly? CheckInFrom { get; set; }
    public DateOnly? CheckOutTo { get; set; }
}
