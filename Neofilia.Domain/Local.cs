using static Neofilia.Domain.Menu;
using static Neofilia.Domain.Table;

namespace Neofilia.Domain;
//Aggregate root
//Local must have many Tables
//Local may own many Menus



public class Local
{
    private readonly List<Table> _tables = [];
    private readonly List<Menu> _menus = [];
    private Local() { } //ef ctor

    public readonly record struct LocalId(int Value);
    public LocalId Id { get; private set; } //PK
    public NotEmptyString Email { get; private set; }
    public NotEmptyString Name { get; private set; }
    public Address Address { get; private set; }
    public DateTimeOffset EventStartsAt { get; private set; }
    public DateTimeOffset EventEndsAt { get; private set; }

    //navigation
    public IReadOnlyList<Table> Tables => _tables.AsReadOnly();
    public IReadOnlyList<Menu> Menus => _menus.AsReadOnly();

    public Local(
        string email,
        string name,
        Address address,
        DateTimeOffset eventStartsAt,
        DateTimeOffset eventEndsAt,
        ICollection<Table> tables,
        ICollection<Menu> menuIds)
    {
        Email = new NotEmptyString(email);
        Name = new NotEmptyString(name);
        Address = address;
        if (eventStartsAt > eventEndsAt)
            throw new InvalidOperationException("Event needs to start before it ends");
        EventStartsAt = eventStartsAt;
        EventEndsAt = eventEndsAt;
        _tables = [.. tables];
        _menus = [.. menuIds];
    }

    //TODO: add domain events and further validation
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
    public void AddPartecipantToTable(Partecipant partecipant, Table table)
    {
        var existingTable = _tables.FirstOrDefault(t => t.Equals(table))
            ?? throw new InvalidOperationException("given table was not found in local: At(AddPartecipantToTable)");

        if (_tables.Any(t => t.Partecipants
                   .Any(p => p == partecipant)))
        {
            var currentTable = _tables.First(t => t.Id.Equals(partecipant.TableId));

            SwitchTable(
                partecipant,
                current: currentTable,
                other: existingTable);
            return;
        }

        existingTable.AddPartecipant(partecipant);
    }
    public void RemovePartecipantFromTable(Partecipant partecipant, Table table) =>
        table.RemovePartecipant(partecipant);


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
    private void SwitchTable(Partecipant partecipant, Table current, Table other)
    {
        current.RemovePartecipant(partecipant);
        other.AddPartecipant(partecipant);
    }


    private Local(
    int id,
    string email,
    string name,
    Address address,
    DateTimeOffset eventStartsAt,
    DateTimeOffset eventEndsAt,
    ICollection<Table> tables,
    ICollection<Menu> menuIds)
    {
        Id = new LocalId(id);
        Email = new NotEmptyString(email);
        Name = new NotEmptyString(name);
        Address = address;
        if (eventStartsAt > eventEndsAt)
            throw new InvalidOperationException("Event needs to start before it ends");
        EventStartsAt = eventStartsAt;
        EventEndsAt = eventEndsAt;
        _tables = [.. tables];
        _menus = [.. menuIds];
    }
    public static Local CreateTestLocal(int id) =>
        new Local(
                id,
                "test@gmail.com",
                "testLocal",
                new Address(new NotEmptyString("test"),
                            new NotEmptyString("test"),
                            new NotEmptyString("test")),
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddDays(1),
                new List<Table>()
                {
                    Table.CreateTestTable(new Table.TableId(1), new Local.LocalId(1), 1)
                },
                new List<Menu>()
                {
                    Menu.CreateTestMenu(new Menu.MenuId(1), new Uri("http://example.com"))
                });
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