using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Application.Interfaces;
using CafeEmployeeManagement.Domain.Constants;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ApiResponse<AuthResponseDto>>
    {
        private readonly IIdentityService identityService;
        private readonly IJwtTokenGenerator jwtTokenGenerator;

        public RegisterCommandHandler(IIdentityService identityService, IJwtTokenGenerator jwtTokenGenerator)
        {
            this.identityService = identityService;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ApiResponse<AuthResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Self-registered users always get the least privileged role. Admins grant other roles.
            var result = await identityService.CreateUserAsync(request.Email, request.Password, Roles.User);

            if (!result.Succeeded)
            {
                return ApiResponse<AuthResponseDto>.SetFailure(result.Errors, "Registration failed");
            }

            var user = await identityService.ValidateCredentialsAsync(request.Email, request.Password);

            if (user == null)
            {
                return ApiResponse<AuthResponseDto>.SetFailure(["Unable to sign in the registered user."], "Registration failed");
            }

            return ApiResponse<AuthResponseDto>.SetSuccess(jwtTokenGenerator.GenerateToken(user), "Registration successful");
        }
    }
}
