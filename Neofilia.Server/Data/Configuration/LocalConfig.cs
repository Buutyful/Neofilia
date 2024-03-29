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
        builder.Property(c => c.Name)
               .HasConversion(
                str => str.Value,
                value => new NotEmptyString(value));
        builder.OwnsOne(x => x.Address, addressBuilder =>
        {
           
        });
    }
}
