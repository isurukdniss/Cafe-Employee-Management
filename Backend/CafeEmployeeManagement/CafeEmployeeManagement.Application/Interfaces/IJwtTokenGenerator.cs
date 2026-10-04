using CafeEmployeeManagement.Application.Features.Auth;
using CafeEmployeeManagement.Application.Common.Models;

namespace CafeEmployeeManagement.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        AuthResponseDto GenerateToken(AuthUser user);
    }
}
