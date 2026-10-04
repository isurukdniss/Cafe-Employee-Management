using CafeEmployeeManagement.Application.Common.Models;

namespace CafeEmployeeManagement.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<IdentityResultModel> CreateUserAsync(string email, string password, string role);

        /// <summary>
        /// Returns the user when the credentials are valid, otherwise null.
        /// </summary>
        Task<AuthUser?> ValidateCredentialsAsync(string email, string password);

        Task<IdentityResultModel> AddToRoleAsync(string email, string role);
    }
}
