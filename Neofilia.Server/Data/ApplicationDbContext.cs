using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Neofilia.Server.Data.Configuration;

namespace Neofilia.Server.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LocalConfig());
        modelBuilder.ApplyConfiguration(new TableConfig());
        modelBuilder.ApplyConfiguration(new MenuConfig());
    }
}
