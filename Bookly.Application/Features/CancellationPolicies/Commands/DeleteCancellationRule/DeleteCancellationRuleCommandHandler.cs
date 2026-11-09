using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.CancellationPolicies.Errors;
using Bookly.Application.Features.CancellationPolicies.Specifications;
using Bookly.Domain.Entities.Cancellation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.CancellationPolicies.Commands.DeleteCancellationRule;

public class DeleteCancellationRuleCommandHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<DeleteCancellationRuleCommandHandler> logger)
    : IRequestHandler<DeleteCancellationRuleCommand, Result>
{
    public async Task<Result> Handle(DeleteCancellationRuleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Attempting to delete cancellation rule {RuleId} for hotel {HotelId}",
            request.RuleId,
            request.HotelId);

        var hotelAccessResult = await hotelAccessService
            .EnsureCurrentUserManagesAsync(request.HotelId, cancellationToken);

        if (hotelAccessResult.IsFailure)
        {
            logger.LogWarning(
                "User is not authorized to manage hotel {HotelId}",
                request.HotelId);

            return hotelAccessResult;
        }

        var specification = new CancellationRuleByIdSpecification(
            request.HotelId,
            request.RuleId);

        var rule = await unitOfWork.Repository<CancellationRule>()
            .GetEntityWithSpecAsync(specification, cancellationToken);

        if (rule is null)
        {
            logger.LogWarning(
                "Cancellation rule {RuleId} not found for hotel {HotelId}",
                request.RuleId,
                request.HotelId);

            return Result.Failure(CancellationErrors.RuleNotFound());
        }

        var hasOtherRules = await unitOfWork.Repository<CancellationRule>()
            .AnyAsync(
                x => x.CancellationPolicyId == rule.CancellationPolicyId &&
                     x.Id != rule.Id,
                cancellationToken);

        if (!hasOtherRules)
        {
            logger.LogWarning(
                "Cannot delete the last cancellation rule {RuleId} from policy {PolicyId}",
                rule.Id,
                rule.CancellationPolicyId);

            return Result.Failure(CancellationErrors.CannotDeleteLastRule());
        }

        unitOfWork.Repository<CancellationRule>().Remove(rule);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Cancellation rule {RuleId} successfully deleted from hotel {HotelId}",
            request.RuleId,
            request.HotelId);

        return Result.Success();
    }
}