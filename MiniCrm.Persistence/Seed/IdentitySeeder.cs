using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniCrm.Application.Common;
using MiniCrm.Persistence.Identity;

namespace MiniCrm.Persistence.Seed;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        using var scope =
            serviceProvider.CreateScope();

        var roleManager =
            scope.ServiceProvider
                .GetRequiredService<
                    RoleManager<IdentityRole<Guid>>>();

        var userManager =
            scope.ServiceProvider
                .GetRequiredService<
                    UserManager<ApplicationUser>>();


        // ========================================================
        // ROLES
        // ========================================================

        var roles = new[]
        {
            AppRoles.Admin,
            AppRoles.Customer
        };

        foreach (var roleName in roles)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                continue;
            }

            var role =
                new IdentityRole<Guid>
                {
                    Id = Guid.NewGuid(),
                    Name = roleName
                };

            var roleResult =
                await roleManager.CreateAsync(role);

            if (!roleResult.Succeeded)
            {
                var errors =
                    string.Join(
                        " | ",
                        roleResult.Errors
                            .Select(x => x.Description));

                throw new InvalidOperationException(
                    $"Role '{roleName}' could not be created: {errors}");
            }
        }


        // ========================================================
        // ADMIN SETTINGS
        // ========================================================

        var adminEmail =
            configuration["Admin:Email"];

        var adminPassword =
            configuration["Admin:Password"];


        // Admin bilgileri ayarlanmamışsa
        // sadece role seed işlemi yapılır.
        if (string.IsNullOrWhiteSpace(adminEmail) ||
            string.IsNullOrWhiteSpace(adminPassword))
        {
            return;
        }

        adminEmail =
            adminEmail.Trim()
                .ToLowerInvariant();


        // ========================================================
        // ADMIN USER
        // ========================================================

        var adminUser =
            await userManager.FindByEmailAsync(
                adminEmail);

        if (adminUser is null)
        {
            adminUser =
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),

                    UserName = adminEmail,

                    Email = adminEmail,

                    EmailConfirmed = true,

                    IsActive = true,

                    LockoutEnabled = true,

                    CreatedAtUtc =
                        DateTime.UtcNow
                };

            var createResult =
                await userManager.CreateAsync(
                    adminUser,
                    adminPassword);

            if (!createResult.Succeeded)
            {
                var errors =
                    string.Join(
                        " | ",
                        createResult.Errors
                            .Select(x => x.Description));

                throw new InvalidOperationException(
                    $"Admin user could not be created: {errors}");
            }
        }


        // ========================================================
        // ADMIN ROLE
        // ========================================================

        var isAdmin =
            await userManager.IsInRoleAsync(
                adminUser,
                AppRoles.Admin);

        if (!isAdmin)
        {
            var addRoleResult =
                await userManager.AddToRoleAsync(
                    adminUser,
                    AppRoles.Admin);

            if (!addRoleResult.Succeeded)
            {
                var errors =
                    string.Join(
                        " | ",
                        addRoleResult.Errors
                            .Select(x => x.Description));

                throw new InvalidOperationException(
                    $"Admin role could not be assigned: {errors}");
            }
        }
    }
}