using CafeEmployeeManagement.Application.Common.Models;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Auth.Commands.AssignRole
{
    public class AssignRoleCommand : IRequest<ApiResponse<string>>
    {
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
