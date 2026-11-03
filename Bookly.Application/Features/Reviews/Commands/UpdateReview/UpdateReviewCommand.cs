using Bookly.Application.Common.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommand : IRequest<Result>
    {
        [JsonIgnore]
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
