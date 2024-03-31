using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Neofilia.Domain;

namespace Neofilia.Server.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Local> Locals => Set<Local>();
    public DbSet<Table> Tables => Set<Table>();
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {       
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);       
    }
}
