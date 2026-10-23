using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Amenities.Commands.DeactivateAmenity;

public record DeactivateAmenityCommand(int Id) : IRequest<Result>;


