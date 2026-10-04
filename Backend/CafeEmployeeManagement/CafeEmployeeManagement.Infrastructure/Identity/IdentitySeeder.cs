using CafeEmployeeManagement.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CafeEmployeeManagement.Infrastructure.Identity
{
    public static class IdentitySeeder
    {
        /// <summary>
        /// Creates the admin account from the "SeedAdmin" configuration section if it doesn't exist yet.
        /// Roles themselves are seeded by the migrations (see SeedDataStore).
        /// </summary>
        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger>();

            var email = configuration["SeedAdmin:Email"];
            var password = configuration["SeedAdmin:Password"];

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            try
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                if (await userManager.FindByEmailAsync(email) != null)
                {
                    return;
                }

                var admin = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
                var result = await userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                {
                    result = await userManager.AddToRolesAsync(admin, Roles.All);
                }

                if (!result.Succeeded)
                {
                    logger.Error("Failed to seed admin user {Email}: {Errors}", email,
                        string.Join("; ", result.Errors.Select(e => e.Description)));
                    return;
                }

                logger.Information("Seeded admin user {Email}", email);
            }
            catch (Exception ex)
            {
                // Most likely the Identity migration hasn't been applied yet. Don't block startup.
                logger.Error(ex, "Failed to seed admin user {Email}", email);
            }
        }
    }
}
