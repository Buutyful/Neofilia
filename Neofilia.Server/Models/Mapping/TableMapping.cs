using Neofilia.Domain;

namespace Neofilia.Server.Models.Mapping;

public static class TableMapping
{
    //TODO: figure out how to handle reward types
    public static TableModel ToModel(this Table table) =>
       new()
       {
           Id = table.Id.Value,
           LocalId = table.LocalId.Value,
           TableNumber = table.TableNumber,
           RewardType = RewardType.Drink
       };
    public static List<TableModel> ToModelList(this IEnumerable<Table> source) =>
        source.Select(ToModel).ToList();
}
