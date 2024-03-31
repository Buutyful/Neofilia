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
    private readonly List<Table> _tables = [];
    private readonly List<Menu> _menus = [];
    private Local() { } //ef ctor

    public readonly record struct LocalId(int Id);
    //public readonly record struct ManagerId(string Id);
    public LocalId Id { get; private set; } //PK
    public string ApplicationUserId { get; private set; } //FK: ApplicationUser{ID}, NOT REQUIRED
    public NotEmptyString Name { get; private set; }
    public Address Address { get; private set; }
    public DateTimeOffset EventStartsAt { get; private set; }
    public DateTimeOffset EventEndsAt { get; private set; }

    //navigation
    public IReadOnlyList<Table> Tables => _tables.AsReadOnly();   
    public IReadOnlyList<Menu> Menus => _menus.AsReadOnly();

    public Local(
        string manager,
        string name,
        Address address,
        DateTimeOffset eventStartsAt,
        DateTimeOffset eventEndsAt,
        ICollection<Table> tables,
        ICollection<Menu> menuIds)
    {
        ApplicationUserId = manager;
        Name = new NotEmptyString(name);
        Address = address;
        if (eventStartsAt > eventEndsAt)
            throw new InvalidOperationException("Event needs to start before it ends");
        EventStartsAt = eventStartsAt;
        EventEndsAt = eventEndsAt;
        _tables = [.. tables];
        _menus = [.. menuIds];
    }

    //need to add domain events and further validation
    #region Table
    public void AddTable(Table table)
    {
        if (_tables.Any(t => t.Equals(table)))
            throw new InvalidOperationException("cant have the same table two times: At(AddTable)");
        _tables.Add(table);
    }
    public void RemoveTable(TableId tableId)
    {
        var table = _tables.FirstOrDefault(t => t.Id == tableId) ??
            throw new InvalidOperationException("Table with given id was not found: At(RemoveTable)");
        _tables.Remove(table);
    }
    public void AddPartecipantToTable(Guid partecipantId, Table table)
    {
        if (_tables.Any(t => t.PartecipantsId
                  .Any(id => id == partecipantId)))
            throw new InvalidOperationException("user already partecipating to an other table: At(AddPartecipantToTable)");
        
        var localTable = _tables.FirstOrDefault(t => t.Equals(table))
            ?? throw new InvalidOperationException("given table was not found in local: At(AddPartecipantToTable)");
        
        localTable.AddPartecipant(partecipantId);
    }
    public void RemovePartecipantFromTable(Guid partecipantId, Table table)
    {
        var localTable = _tables.FirstOrDefault(t => t.Equals(table))
            ?? throw new InvalidOperationException("given table was not found in local");

        localTable.RemovePartecipant(partecipantId);
    }

    #endregion

    public void AddMenu(Menu menu)
    {
        if (_menus.Any(m => m.Id.Equals(menu.Id)))
            throw new InvalidOperationException("cant have the same menu two times");
        _menus.Add(menu);
    }
    public void RemoveMenu(MenuId menuId)
    {
        var menu = _menus.Where(m => m.Id.Equals(menuId)).First();
        _menus.Remove(menu);
    }
    public void ChangeEventStartDate(DateTimeOffset date) => EventStartsAt = date;
    public void ChangeEventEndDate(DateTimeOffset date) => EventEndsAt = date;

}

public readonly record struct NotEmptyString(string Value)
{
    public string Value { get; init; } =
        !string.IsNullOrWhiteSpace(Value) ? Value.Trim()
        : throw new ArgumentException("Value cannot be null or white space", nameof(Value));
    public NotEmptyString() : this("NoData") { } 
    
    public static implicit operator string(NotEmptyString value) => value.Value;
}

public record Address(NotEmptyString Street, NotEmptyString CivilNumber, NotEmptyString PhoneNumber);