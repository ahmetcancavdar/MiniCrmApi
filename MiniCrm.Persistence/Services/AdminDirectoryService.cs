using Microsoft.AspNetCore.Identity;
using MiniCrm.Application.Common;
using MiniCrm.Application.Interfaces.Services;
using MiniCrm.Persistence.Identity;

namespace MiniCrm.Persistence.Services;

public sealed class AdminDirectoryService
    : IAdminDirectoryService
{
    private readonly UserManager<ApplicationUser>
        _userManager;


    public AdminDirectoryService(
        UserManager<ApplicationUser> userManager)
    {
        _userManager =
            userManager;
    }


    public async Task<List<string>> GetAdminEmailsAsync(
        CancellationToken cancellationToken = default)
    {
        var admins =
            await _userManager.GetUsersInRoleAsync(
                AppRoles.Admin);

        return admins
            .Where(x =>
                !string.IsNullOrWhiteSpace(x.Email))
            .Select(x =>
                x.Email!)
            .ToList();
    }
}
