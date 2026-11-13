using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Images.Commands.DeleteHotelImage;

public record DeleteHotelImageCommand(int HotelId, int ImageId)
    : IRequest<Result>;
