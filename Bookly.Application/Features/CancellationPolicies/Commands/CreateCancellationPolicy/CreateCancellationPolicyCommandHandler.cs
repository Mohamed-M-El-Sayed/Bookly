using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.CancellationPolicies.Specifications;
using Bookly.Application.Features.Hotels.Errors;
using Bookly.Domain.Entities.Cancellation;
using Bookly.Domain.Entities.Hotels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.CancellationPolicies.Commands.CreateCancellationPolicy;

public class CreateCancellationPolicyCommandHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<CreateCancellationPolicyCommandHandler> logger) : IRequestHandler<CreateCancellationPolicyCommand, Result>
{
    public async Task<Result> Handle(CreateCancellationPolicyCommand request, CancellationToken cancellationToken)
    {
        var hotelAccessResult = await hotelAccessService.EnsureCurrentUserManagesAsync(request.HotelId, cancellationToken);
        if (hotelAccessResult.IsFailure)
            return hotelAccessResult;

        var hotel = await unitOfWork.Repository<Hotel>()
            .GetEntityWithSpecAsync(new HotelCancellationRulesSpecification(request.HotelId), cancellationToken);
        if (hotel is null)
            return Result.Failure(HotelErrors.NotFound());

        var policy = hotel.CancellationPolicy
          ?? CancellationPolicy.Create(request.HotelId);

        policy.AddRules(request.Rules.Select(rule =>
            (rule.DaysBeforeCheckIn, rule.RefundPercentage)));

        if (hotel.CancellationPolicy is null)
        {
            await unitOfWork.Repository<CancellationPolicy>().AddAsync(policy, cancellationToken);
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Cancellation policy created successfully for hotel {HotelId}", request.HotelId);
        return Result.Success();
    }
}