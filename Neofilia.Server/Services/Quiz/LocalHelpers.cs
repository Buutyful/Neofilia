using Neofilia.Domain;
using Neofilia.Server.Data.Repository;

namespace Neofilia.Server.Services.Quiz;

public static class LocalHelpers
{
    public static async Task<List<Local>> GetLocalsAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var localRepo = scope.ServiceProvider.GetRequiredService<ILocalRepository>();
        var implementation = localRepo as LocalRepository;
        return await implementation!.GetLocalsWithTables();
    }
}