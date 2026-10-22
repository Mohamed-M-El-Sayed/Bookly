using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Amenities.Commands.DeleteAmenity;

public record DeleteAmenityCommand(int AmenityId) : IRequest<Result>;
