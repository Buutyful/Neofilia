namespace Neofilia.Domain;

//use local time?
public class Reward
{
    private Reward() { } //ef ctor
    public readonly record struct RewardId(int Id);
    public RewardId Id { get; set; } //PK
    public RewardType Type { get; private set; } = RewardType.None;
    public bool IsReedemed { get; private set; }
    public DateTimeOffset GeneratedAt { get; private set; }
    public DateTimeOffset ReedemedAt { get; private set; }

    public Reward(RewardType type)
    {
        Type = type;
        GeneratedAt = DateTimeOffset.Now;
    }
    
    public void Reedem()
    {
        if (IsReedemed) throw new InvalidOperationException("reward already redeemed");
        IsReedemed = true;
        ReedemedAt = DateTimeOffset.Now;
    }
}

public enum RewardType
{
    None,
    Drink,
    Money
}