using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neofilia.Domain;

namespace Neofilia.Server.Data.Configuration;

public class RewardConfig : IEntityTypeConfiguration<Reward>
{
    public void Configure(EntityTypeBuilder<Reward> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(c => c.Id)
               .HasConversion(
                rewardId => rewardId.Id,
                value => new Reward.RewardId(value));

        builder.Property(c => c.Money)
               .HasConversion(
                money => money.Amount,
                value => new Money(value));
              
    }
}
