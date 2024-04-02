using Neofilia.Domain;
using Neofilia.Server.Data.Repository;

namespace Neofilia.Server.Data;

public static class PersistenceExtentions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Local>, LocalRepository>();
        services.AddScoped<IRepository<Table>, TableRepository>();
        return services;
    }
}
