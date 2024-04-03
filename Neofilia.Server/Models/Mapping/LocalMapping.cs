using Neofilia.Domain;

namespace Neofilia.Server.Models.Mapping;

public static class LocalMapping
{
    public static LocalModel ToModel(this Local local) =>
        new()
        {
            Id = local.Id.Value,
            ApplicationUserId = local.ApplicationUserId,
            Name = local.Name,
            Street = local.Address.Street,
            CivilNumber = local.Address.CivilNumber,
            PhoneNumber = local.Address.PhoneNumber,
            EventStartsAt = local.EventStartsAt,
            EventEndsAt = local.EventEndsAt,
        };
    public static List<LocalModel> ToModelList(this IEnumerable<Local> source) =>
        source.Select(ToModel).ToList();
}
