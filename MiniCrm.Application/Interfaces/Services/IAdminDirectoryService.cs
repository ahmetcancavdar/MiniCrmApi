namespace MiniCrm.Application.Interfaces.Services;

public interface IAdminDirectoryService
{
    Task<List<string>> GetAdminEmailsAsync(
        CancellationToken cancellationToken = default);
}
