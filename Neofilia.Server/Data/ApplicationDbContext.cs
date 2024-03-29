using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Neofilia.Domain;
using Neofilia.Server.Data.Configuration;

namespace Neofilia.Server.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Table> Tables => Set<Table>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<Local> Locals => Set<Local>();
    public DbSet<Reward> Rewards => Set<Reward>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);       
    }
}
