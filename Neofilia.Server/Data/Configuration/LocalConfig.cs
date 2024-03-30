using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neofilia.Domain;

namespace Neofilia.Server.Data.Configuration;

public class LocalConfig : IEntityTypeConfiguration<Local>
{
    public void Configure(EntityTypeBuilder<Local> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(c => c.Id)
               .HasConversion(
                local => local.Id,
                value => new Local.LocalId(value));

        builder.Property(c => c.Name).ConfigureNotEmptyString();

        builder.OwnsOne(x => x.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.Street)
                .HasColumnName("Street")
                .ConfigureNotEmptyString();

            addressBuilder.Property(a => a.CivilNumber)
                .HasColumnName("CivilNumber")
                .ConfigureNotEmptyString();

            addressBuilder.Property(a => a.PhoneNumber)
                .HasColumnName("PhoneNumber")
                .ConfigureNotEmptyString();
        });

        builder.HasMany(t => t.Tables)
               .WithOne()
               .HasForeignKey(t => t.LocalId);

        builder.OwnsMany(m => m.Menus, menuBuilder =>
        {
            menuBuilder.ToTable("Menus");

            menuBuilder.HasKey("Id", "LocalId");

            menuBuilder.WithOwner()
                       .HasForeignKey(t => t.LocalId);

            menuBuilder.Property(c => c.Id)
                       .HasConversion(
                        menuId => menuId.Id,
                        value => new Menu.MenuId(value));
        });
    }
}
