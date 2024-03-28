using static Neofilia.Domain.Menu;
using static Neofilia.Domain.Table;

namespace Neofilia.Domain;
//Aggregate root
//Locale must have many Tables
//Locale may have many Menus
//UserManager may have many Locale
//how should the global game be implemented?

public class Local
{
    public record struct LocalId(int Id);
    public LocalId Id { get; private set; } //PK
    public Guid? ManagerId { get; private set; } //FK: ApplicationUser{ID}, NOT REQUIRED
    public NotEmptyString Name { get; private set; }
    public Adress Adress { get; private set; }
    public DateTimeOffset EventStartsAt { get; private set; }
    public DateTimeOffset EventEndsAt { get; private set; }

    //navigation
    public ICollection<Table> Tables { get; private set; } = [];
    public ICollection<MenuId> Menus { get; private set; } = [];
    public IEnumerable<Table> GetTables => Tables;
    public IEnumerable<MenuId> MenuIds => Menus;

    public Local(
        Guid manager,
        string name,
        Adress adress,
        DateTimeOffset eventStartsAt,
        DateTimeOffset eventEndsAt,
        ICollection<Table> tables,
        ICollection<MenuId> menuIds)
    {
        ManagerId = manager;
        Name = new NotEmptyString(name);
        Adress = adress;
        if (EventStartsAt < EventEndsAt)
            throw new InvalidOperationException("Event needs to start before it ends");
        EventStartsAt = eventStartsAt;
        EventEndsAt = eventEndsAt;
        Tables = [.. tables];
        Menus = [.. menuIds];
    }

    //need to add domain events and further validation
    #region Table
    public void AddTable(Table table)
    {
        if (Tables.Any(t => t.Equals(table)))
            throw new InvalidOperationException("cant have the same table two times");
        Tables.Add(table);
    }
    public void RemoveTable(TableId tableId)
    {
        var table = Tables.Where(t => t.Id == tableId).First();
        Tables.Remove(table);
    }
    public void AddPartecipantToTable(Guid partecipantId, Table table)
    {
        if (Tables.Any(t => t.PartecipantsId
                  .Any(id => id == partecipantId)))
            throw new InvalidOperationException("user already partecipating to an other table");
        table.AddPartecipant(partecipantId);
    }
    public void RemovePartecipantFromTable(Guid partecipantId, Table table) => 
        table.RemovePartecipant(partecipantId);
    #endregion

    public void AddMenu(MenuId menuId)
    {
        if (Menus.Contains(menuId))
            throw new InvalidOperationException("cant have the same menu two times");
        Menus.Add(menuId);
    }
    public void RemoveMenu(MenuId menuId)
    {
        var menu = Menus.Where(m => m == menuId).First();
        Menus.Remove(menu);
    }
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