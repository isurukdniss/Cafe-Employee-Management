using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Application.Interfaces;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Auth.Commands.AssignRole
{
    public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, ApiResponse<string>>
    {
        private readonly IIdentityService identityService;

        public AssignRoleCommandHandler(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        public async Task<ApiResponse<string>> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await identityService.AddToRoleAsync(request.Email, request.Role);

            if (!result.Succeeded)
            {
                return ApiResponse<string>.SetFailure(result.Errors, "Role assignment failed");
            }

            return ApiResponse<string>.SetSuccess(request.Role, $"Role '{request.Role}' assigned to {request.Email}");
        }
    }
}
