using Bookly.Application.Common.Results;
using MediatR;

using System.Text.Json.Serialization;

namespace Bookly.Application.Features.CancellationPolicies.Commands.CreateCancellationPolicy;

public class CreateCancellationPolicyCommand : IRequest<Result>
{
    [JsonIgnore]
    public int HotelId { get; set; }
    public IReadOnlyCollection<CancellationRuleRequest> Rules { get; set; } = [];

}
public record CancellationRuleRequest(
    int DaysBeforeCheckIn,
    decimal RefundPercentage
);