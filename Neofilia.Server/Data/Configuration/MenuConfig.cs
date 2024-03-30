using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neofilia.Domain;

namespace Neofilia.Server.Data.Configuration;

public class MenuConfig : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        throw new NotImplementedException();
    }
}
