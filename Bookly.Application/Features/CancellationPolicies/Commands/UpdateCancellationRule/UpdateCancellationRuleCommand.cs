using Bookly.Application.Common.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.CancellationPolicies.Commands.UpdateCancellationRule;

public class UpdateCancellationRuleCommand : IRequest<Result>
{
    [JsonIgnore]
    public int HotelId { get; set; }
    [JsonIgnore]
    public int RuleId { get; set; }
    public int DaysBeforeCheckIn { get; set; }
    public decimal RefundPercentage { get; set; }
}
