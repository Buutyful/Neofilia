using Neofilia.Domain;
using Neofilia.Server.Data.Repository;

namespace Neofilia.Server.Data;

public static class PersistenceExtentions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<ILocalRepository, LocalRepository>();
        services.AddScoped<ITableRepository, TableRepository>();
        return services;
    }
}
