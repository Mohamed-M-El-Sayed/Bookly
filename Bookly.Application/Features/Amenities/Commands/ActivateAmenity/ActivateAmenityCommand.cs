using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Amenities.Commands.ActivateAmenity;

public record ActivateAmenityCommand(int Id) : IRequest<Result>;
