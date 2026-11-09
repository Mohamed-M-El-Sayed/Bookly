using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.CancellationPolicies.Errors;
using Bookly.Domain.Entities.Cancellation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.CancellationPolicies.Commands.UpdateCancellationRule;

public class UpdateCancellationRuleCommandHandler(
    IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<UpdateCancellationRuleCommandHandler> logger) : IRequestHandler<UpdateCancellationRuleCommand, Result>
{
    public async Task<Result> Handle(UpdateCancellationRuleCommand request, CancellationToken cancellationToken)
    {
        var hotelAccessResult = await hotelAccessService.EnsureCurrentUserManagesAsync(
                request.HotelId, cancellationToken);

        if (hotelAccessResult.IsFailure)
            return hotelAccessResult;

        var rule = await unitOfWork.Repository<CancellationRule>()
            .GetByIdAsync(request.RuleId, cancellationToken);

        if (rule is null)
            return Result.Failure(CancellationErrors.RuleNotFound());

        var belongsToHotel = await unitOfWork.Repository<CancellationRule>()
            .AnyAsync(x => x.Id == request.RuleId &&
                     x.CancellationPolicy.HotelId == request.HotelId,
                cancellationToken);

        if (!belongsToHotel)
            return Result.Failure(CancellationErrors.RuleNotFound());

        // Check whether another rule already uses the requested threshold.
        var thresholdExists = await unitOfWork.Repository<CancellationRule>()
            .AnyAsync(
                x => x.CancellationPolicyId == rule.CancellationPolicyId &&
                     x.DaysBeforeCheckIn == request.DaysBeforeCheckIn &&
                     x.Id != rule.Id,
                cancellationToken);

        if (thresholdExists)
            return Result.Failure(CancellationErrors.ThresholdAlreadyExists());
        rule.Update(
            request.DaysBeforeCheckIn,
            request.RefundPercentage);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Cancellation rule {RuleId} updated for hotel {HotelId}",
            request.RuleId,
            request.HotelId);

        return Result.Success();
    }
}