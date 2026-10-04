using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Application.Interfaces;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<AuthResponseDto>>
    {
        private readonly IIdentityService identityService;
        private readonly IJwtTokenGenerator jwtTokenGenerator;

        public LoginCommandHandler(IIdentityService identityService, IJwtTokenGenerator jwtTokenGenerator)
        {
            this.identityService = identityService;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ApiResponse<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await identityService.ValidateCredentialsAsync(request.Email, request.Password);

            if (user == null)
            {
                // Same message for unknown email, wrong password and lockout, so accounts can't be enumerated.
                return ApiResponse<AuthResponseDto>.SetFailure(["Invalid email or password."], "Login failed");
            }

            return ApiResponse<AuthResponseDto>.SetSuccess(jwtTokenGenerator.GenerateToken(user), "Login successful");
        }
    }
}
