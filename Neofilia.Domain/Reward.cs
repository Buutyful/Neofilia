﻿using static Neofilia.Domain.Table;

namespace Neofilia.Domain;

//use local time?
//should a local have his own rewards?
//should a reward be a base class for other rewards?
//TODO: implement decorator pattern for different rewards types
public class Reward
{
    private Reward() { } //ef ctor
    public readonly record struct RewardId(int Id);
    public RewardId Id { get; private set; } //PK
    public TableId TableId { get; private set; } //FK: Tables{ID}, REQUIRED
    public RewardType Type { get; private set; } = RewardType.None;
    public Money Money { get; init; }
    public bool IsRedeemed { get; private set; }
    public DateTimeOffset GeneratedAt { get; private set; }
    public DateTimeOffset RedeemedAt { get; private set; }

    public Reward(RewardType type, TableId tableId)
    {
        Type = type;
        TableId = tableId;
        Money = new Money(0);
        GeneratedAt = DateTimeOffset.Now;
    }
    private Reward(RewardType type, TableId tableId, Money money)
    {
        Type = type;
        TableId = tableId;
        GeneratedAt = DateTimeOffset.Now;
        Money = money;
    }

    public void Redeem()
    {
        if (IsRedeemed) throw new InvalidOperationException("reward already redeemed");
        IsRedeemed = true;
        RedeemedAt = DateTimeOffset.Now;
    }
    public static Reward NewMoneyReward(decimal amount, TableId tableId) =>
        new(RewardType.Money, tableId, new Money(amount));
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
        if (amount < 0) throw new InvalidOperationException("Money amount cannot be negative");
        Amount = Math.Round(amount, 2);
    }
}