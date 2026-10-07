using Bookly.Application.Common.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<Result>
    {
        [JsonIgnore]
        public string UserId { get; set; } = string.Empty;
        [JsonIgnore]
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
