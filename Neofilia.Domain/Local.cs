using static Neofilia.Domain.Menu;
using static Neofilia.Domain.Table;

namespace Neofilia.Domain;
//Aggregate root
//Local must have many Tables
//Local may have many Menus
//UserManager may have many Locals
//how should the global game be implemented?

public class Local
{
    private Local() { } //ef ctor

    public readonly record struct LocalId(int Id);
    public LocalId Id { get; private set; } //PK
    public Guid? ManagerId { get; private set; } //FK: ApplicationUser{ID}, NOT REQUIRED
    public NotEmptyString Name { get; private set; }
    public Address Address { get; private set; }
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
        Address address,
        DateTimeOffset eventStartsAt,
        DateTimeOffset eventEndsAt,
        ICollection<Table> tables,
        ICollection<MenuId> menuIds)
    {
        ManagerId = manager;
        Name = new NotEmptyString(name);
        Address = address;
        if (eventStartsAt > eventEndsAt)
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
            throw new InvalidOperationException("cant have the same table two times: At(AddTable)");
        Tables.Add(table);
    }
    public void RemoveTable(TableId tableId)
    {
        var table = Tables.FirstOrDefault(t => t.Id == tableId) ??
            throw new InvalidOperationException("Table with given id was not found: At(RemoveTable)");
        Tables.Remove(table);
    }
    public void AddPartecipantToTable(Guid partecipantId, Table table)
    {
        if (Tables.Any(t => t.PartecipantsId
                  .Any(id => id == partecipantId)))
            throw new InvalidOperationException("user already partecipating to an other table: At(AddPartecipantToTable)");
        
        var localTable = Tables.FirstOrDefault(t => t.Equals(table))
            ?? throw new InvalidOperationException("given table was not found in local: At(AddPartecipantToTable)");
        
        localTable.AddPartecipant(partecipantId);
    }
    public void RemovePartecipantFromTable(Guid partecipantId, Table table)
    {
        var localTable = Tables.FirstOrDefault(t => t.Equals(table))
            ?? throw new InvalidOperationException("given table was not found in local");

        localTable.RemovePartecipant(partecipantId);
    }

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
    //when creating migration ef will call this and cause exe, just comment it off
    public static implicit operator string(NotEmptyString value) => value.Value;
}

public record Address(NotEmptyString Street, NotEmptyString CivilNumber, NotEmptyString PhoneNumber);