using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace CafeEmployeeManagement.API.Extensions
{
    public static class AuthenticationExtensions
    {
        // Same casing as the controller responses.
        private static readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web);

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>() ?? new JwtSettings();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // Keep the claim names as issued ("sub", "email", "role") instead of mapping them to long URIs.
                    options.MapInboundClaims = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1),
                        NameClaimType = JwtRegisteredClaimNames.Email,
                        RoleClaimType = JwtTokenGenerator.RoleClaimType,
                    };

                    // Return the same ApiResponse body as the rest of the API instead of an empty 401/403.
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();
                            await WriteErrorAsync(context.Response, StatusCodes.Status401Unauthorized,
                                "Unauthorized", "A valid bearer token is required.");
                        },
                        OnForbidden = context => WriteErrorAsync(context.Response, StatusCodes.Status403Forbidden,
                            "Forbidden", "You do not have permission to perform this action."),
                    };
                });

            services.AddAuthorization();

            return services;
        }

        private static Task WriteErrorAsync(HttpResponse response, int statusCode, string message, string error)
        {
            response.StatusCode = statusCode;
            response.ContentType = "application/json";

            var body = new ApiResponse<string>
            {
                Success = false,
                Message = message,
                Errors = [error],
            };

            return response.WriteAsync(JsonSerializer.Serialize(body, jsonOptions));
        }
    }
}
