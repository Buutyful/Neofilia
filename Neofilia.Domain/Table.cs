using static Neofilia.Domain.Local;


namespace Neofilia.Domain;
//Table is an entity, must be accessed only inside Local aggregate boundry

//Table may own one reward
//Table may have many partecipants
// ?? Table may have many local games ?? not now, maybe future feature

//current idea: table is a lobby it self, when partecipants gain points on the runing challenge they add score points to the table lobby,
//when a threshold is reached the table creates a reward that can be redeemed, table can have only 1 reward at a time)

public class Table : IEquatable<Table>
{
    private readonly List<Guid> _usersId = [];
    private Table() { } //ef ctor
    public readonly record struct TableId(int Id);
    public TableId Id { get; private set; } //PK
    public LocalId LocalId { get; private set; } //FK: Locals{ID}, REQUIRED
    public int TableNumber { get; private set; } //should this match the id?

    //navigation
    public Reward? Reward { get; private set; }
    public IReadOnlyCollection<Guid> PartecipantsIds => _usersId.AsReadOnly();

    public Table(LocalId localId, int tableNumber) => 
        (LocalId, TableNumber) = (localId, tableNumber);
    public void AddPartecipant(Guid userId) => _usersId.Add(userId);
    public void RemovePartecipant(Guid userId) => _usersId.Remove(userId);
    public void GenerateReward()
    {
        //TODO
    }
    public bool Equals(Table? other) =>
        other is not null && Id.Equals(other.Id);
    public override bool Equals(object? obj) =>
        obj is Table table && Equals(table);
    public static bool operator ==(Table left, Table right) => left.Equals(right);
    public static bool operator !=(Table left, Table right) => !left.Equals(right);
    public override int GetHashCode() => Id.GetHashCode();

    private Table(TableId id, LocalId localId, int tableNumber) =>
        (Id, LocalId, TableNumber) = (id, localId, tableNumber);
    private Table(TableId id) => Id = id;
    public static Table CreateTestTable(TableId id, LocalId localId, int tableNumber) =>
        new(id, localId, tableNumber);
    public static Table CreateTestTableId(TableId id) =>
       new(id);
}
