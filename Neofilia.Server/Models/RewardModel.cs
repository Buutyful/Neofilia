using Neofilia.Domain;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using static Neofilia.Domain.Table;

namespace Neofilia.Server.Models;

public class RewardModel
{    
    public int Id { get; set; }

    public int TableId { get; set; }

    [Required(ErrorMessage = "Type is required")]
    public RewardType Type { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "MoneyAmount must be a non-negative number")]
    public decimal MoneyAmount { get; set; }

    public DateTimeOffset GeneratedAt { get; set; }

    public DateTimeOffset RedeemedAt { get; set; }


    public Reward ToReward()
    {
        if (Type == RewardType.Money)
        {
            return Reward.NewMoneyReward(MoneyAmount, new TableId(TableId));
        }
        else
        {
            return new Reward(Type, new TableId(TableId));
        }
    }

    public static RewardModel? FromJson(string json)
    {
        if(string.IsNullOrWhiteSpace(json))
            throw new ArgumentException("Value cannot be null or white space", nameof(json));

        return JsonSerializer.Deserialize<RewardModel>(json);
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}

