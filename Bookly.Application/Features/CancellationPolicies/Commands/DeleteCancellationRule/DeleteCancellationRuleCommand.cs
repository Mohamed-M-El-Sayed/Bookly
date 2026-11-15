using Bookly.Application.Common.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.CancellationPolicies.Commands.DeleteCancellationRule;

public class DeleteCancellationRuleCommand : IRequest<Result>
{
    [JsonIgnore]
    public int HotelId { get; set; }

    [JsonIgnore]
    public int RuleId { get; set; }
}
