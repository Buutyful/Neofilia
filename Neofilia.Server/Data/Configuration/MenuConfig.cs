using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neofilia.Domain;

namespace Neofilia.Server.Data.Configuration;

public class MenuConfig : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(c => c.Id)
               .HasConversion(
                menuId => menuId.Id,
                value => new Menu.MenuId(value));       
    }
}
