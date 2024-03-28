using static Neofilia.Domain.Local;


namespace Neofilia.Domain;
//Table is an entity, must be accessed only inside Local aggregate boundry
//Evrey table will subscribe to the game event ? or the user should? or the local should?
//Table may have many local games
//Table may have many partecipants
//(current idea: table is a lobby it self, when partecipants win the on going challenge they add score points to the table lobby,
//when a trashold is reached the table creates a reward that can be redeemed, table can have only 1 reward)
//Table may own one reward

public class Table
{
    private Table() { } //ef ctor
    public readonly record struct TableId(int Id);
    public TableId Id { get; private set; } //PK
    public LocalId LocalId { get; private set; } //FK: Locals{ID}, REQUIRED
    public int TableNumber { get; private set; } //should this match the id?
    public Reward? Reward { get; private set; }

    //navigation
    public ICollection<Guid> UsersId { get; private set; } = [];
    public IEnumerable<Guid> PartecipantsId => UsersId;

    public Table(LocalId localId, int tableNumber) => 
        (LocalId, TableNumber) = (localId, tableNumber);

    public void AddPartecipant(Guid userId) => UsersId.Add(userId);
    public void RemovePartecipant(Guid userId) => UsersId.Remove(userId);
    public bool Equals(Table other) =>
        other != null && Id.Equals(other.Id);
    public override bool Equals(object? obj) =>
        obj is Table table && Equals(table);

    public override int GetHashCode() => Id.GetHashCode();
   

}
