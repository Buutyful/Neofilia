using static Neofilia.Domain.Menu;
using static Neofilia.Domain.Table;

namespace Neofilia.Domain;

//Locale must have many Tables
//Locale may have many Menus
//Locale may have an UserManager
//how should the global game be implemented? 
public class Locale
{
    public record struct LocaleId(int Id);
    public LocaleId Id { get; private set; }
    public Guid? ManagerId { get; private set; }
    public NotEmptyString Name { get; private set; }
    public Adress Adress { get; private set; }
    public DateTimeOffset EventStartsAt { get; private set; }
    public DateTimeOffset EventEndsAt { get; private set; }

    //navigation
    public ICollection<TableId> Tables { get; private set; } = [];
    public ICollection<MenuId> Menus { get; private set; } = [];
    public IEnumerable<TableId> TableIds => Tables;
    public IEnumerable<MenuId> MenuIds => Menus;

    public Locale(
        Guid manager,
        string name,
        Adress adress,
        DateTimeOffset eventStartsAt,
        DateTimeOffset eventEndsAt,
        ICollection<TableId> tableIds,
        ICollection<MenuId> menuIds)
    {
        ManagerId = manager;
        Name = new NotEmptyString(name);
        Adress = adress;
        if (EventStartsAt < EventEndsAt)
            throw new InvalidOperationException("Event needs to start before it ends");
        EventStartsAt = eventStartsAt;
        EventEndsAt = eventEndsAt;
        Tables = [.. tableIds];
        Menus = [.. menuIds];
    }


    public void AddTable(TableId tableId)
    {
        if (Tables.Contains(tableId))
            throw new InvalidOperationException("cant have the same table two times");
        Tables.Add(tableId);
    }
    public void AddMenu(MenuId menuId)
    {
        if (Menus.Contains(menuId))
            throw new InvalidOperationException("cant have the same menu two times");
        Menus.Add(menuId);
    }
    public void RemoveTable(TableId table) => Tables.Remove(table);
    public void ChangeEventStartDate(DateTimeOffset date) => EventStartsAt = date;
    public void ChangeEventEndDate(DateTimeOffset date) => EventEndsAt = date;

}

public readonly record struct NotEmptyString(string Value)
{
    public string Value { get; init; } =
        !string.IsNullOrWhiteSpace(Value) ? Value.Trim()
        : throw new ArgumentException("Value cannot be null or white space", nameof(Value));
    public NotEmptyString() : this(string.Empty) { } //throw if called

    public static implicit operator string(NotEmptyString value) => value.Value;
}

public record Adress(NotEmptyString Street, NotEmptyString CivilNumber, NotEmptyString PhoenNumber);