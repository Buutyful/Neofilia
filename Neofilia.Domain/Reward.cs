namespace Neofilia.Domain;

//use local time?
public class Reward
{
    private Reward() { } //ef ctor
    public readonly record struct RewardId(int Id);
    public RewardId Id { get; private set; } //PK
    public RewardType Type { get; private set; } = RewardType.None;
    public Money? Money { get; init; }
    public bool IsRedeemed { get; private set; }
    public DateTimeOffset GeneratedAt { get; private set; }
    public DateTimeOffset RedeemedAt { get; private set; }

    public Reward(RewardType type)
    {
        Type = type;
        GeneratedAt = DateTimeOffset.Now;
    }
    private Reward(RewardType type, Money money)
    {
        Type = type;
        GeneratedAt = DateTimeOffset.Now;
        Money = money;
    }

    public void Redeem()
    {
        if (IsRedeemed) throw new InvalidOperationException("reward already redeemed");
        IsRedeemed = true;
        RedeemedAt = DateTimeOffset.Now;
    }
    public static Reward NewMoneyReward(decimal amount) =>
        new(RewardType.Money, new Money(amount));
}

public enum RewardType
{
    None,
    Drink,
    Money
}
public record Money
{
    public decimal Amount { get; }   
    public Money(decimal amount)
    {
        if(amount < 0) throw new InvalidOperationException("Money amount cannot be negative");
        Amount = Math.Round(amount, 2);
    }
}