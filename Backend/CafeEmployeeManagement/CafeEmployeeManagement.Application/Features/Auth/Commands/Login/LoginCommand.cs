using CafeEmployeeManagement.Application.Common.Models;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<ApiResponse<AuthResponseDto>>, ISensitiveRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
