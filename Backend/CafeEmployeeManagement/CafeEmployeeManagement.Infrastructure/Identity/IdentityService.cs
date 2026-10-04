using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CafeEmployeeManagement.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IdentityResultModel> CreateUserAsync(string email, string password, string role)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
            };

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return ToModel(result);
            }

            return ToModel(await userManager.AddToRoleAsync(user, role));
        }

        public async Task<AuthUser?> ValidateCredentialsAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null || await userManager.IsLockedOutAsync(user))
            {
                return null;
            }

            if (!await userManager.CheckPasswordAsync(user, password))
            {
                await userManager.AccessFailedAsync(user);
                return null;
            }

            await userManager.ResetAccessFailedCountAsync(user);

            return new AuthUser
            {
                Id = user.Id,
                Email = user.Email!,
                Roles = await userManager.GetRolesAsync(user),
            };
        }

        public async Task<IdentityResultModel> AddToRoleAsync(string email, string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                return IdentityResultModel.Failure([$"Role '{role}' does not exist."]);
            }

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return IdentityResultModel.Failure([$"User '{email}' was not found."]);
            }

            if (await userManager.IsInRoleAsync(user, role))
            {
                return IdentityResultModel.Success();
            }

            return ToModel(await userManager.AddToRoleAsync(user, role));
        }

        private static IdentityResultModel ToModel(IdentityResult result) =>
            result.Succeeded
                ? IdentityResultModel.Success()
                : IdentityResultModel.Failure(result.Errors.Select(e => e.Description));
    }
}
