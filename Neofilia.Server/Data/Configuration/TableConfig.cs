using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neofilia.Domain;


namespace Neofilia.Server.Data.Configuration;

public class TableConfig : IEntityTypeConfiguration<Table>
{
    public void Configure(EntityTypeBuilder<Table> builder)
    {

        builder.HasKey(x => x.Id);

        builder.Property(c => c.Id)
               .HasConversion(
                table => table.Value,
                value => new Table.TableId(value));

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd()
               .UseIdentityColumn();

        builder.OwnsOne(t => t.Reward, rewardBuilder =>
        {
            rewardBuilder.ToTable("Rewards");

            rewardBuilder.HasKey(nameof(Reward.Id), "TableId");


            rewardBuilder.WithOwner()
                         .HasForeignKey(r => r.TableId);

            rewardBuilder.Property(c => c.Id)
                         .HasColumnName("RewardId")
                         .HasConversion(
                          rewardId => rewardId.Id,
                          value => new Reward.RewardId(value));

            rewardBuilder.Property(x => x.Id)
              .ValueGeneratedOnAdd()
              .UseIdentityColumn();

            rewardBuilder.Property(r => r.Type)
                         .HasConversion(
                          type => type.ToString(),
                          value => (RewardType)Enum.Parse(typeof(RewardType), value));

            rewardBuilder.Property(c => c.Money)
                         .HasConversion(
                          money => money.Amount,
                          value => new Money(value));
        });
    }
}
