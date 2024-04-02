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
                local => local.Value,
                value => new Local.LocalId(value));

        builder.Property(c => c.Name).ConfigureNotEmptyString();

        builder.OwnsOne(x => x.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.Street)
                          .ConfigureNotEmptyString();

            addressBuilder.Property(a => a.CivilNumber)
                          .ConfigureNotEmptyString();

            addressBuilder.Property(a => a.PhoneNumber)
                          .ConfigureNotEmptyString();
        });

        builder.HasMany(t => t.Tables)
               .WithOne()
               .HasForeignKey(t => t.LocalId)
               .IsRequired();


        builder.OwnsMany(m => m.Menus, menuBuilder =>
        {
            menuBuilder.ToTable("Menus");

            menuBuilder.HasKey(nameof(Menu.Id), "LocalId");

            menuBuilder.WithOwner()
                       .HasForeignKey("LocalId");

            menuBuilder.Property(c => c.Id)
                       .HasColumnName("MenuId")
                       .HasConversion(
                        menuId => menuId.Id,
                        value => new Menu.MenuId(value));
        });

        builder.Metadata.FindNavigation(nameof(Local.Tables))!
                        .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata.FindNavigation(nameof(Local.Menus))!
                        .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
