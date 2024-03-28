using static Neofilia.Domain.Local;


namespace Neofilia.Domain;
//Table is an entity, accessible inside Local aggregate boundry
//Table must have a global Local game
//Table may have many local games
//Table may have many partecipants : how to implement this? table should host a lobby?

public class Table
{
    public record struct TableId(int Id);
    public TableId Id { get; private set; } //PK
    public LocalId LocalId { get; private set; } //FK: Locals{ID}, REQUIRED
    public int TableNumber { get; private set; } //should this match the id?


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
