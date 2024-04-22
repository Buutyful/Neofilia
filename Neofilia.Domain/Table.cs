using System.ComponentModel.DataAnnotations.Schema;
using static Neofilia.Domain.Local;
using static Neofilia.Domain.Table;


namespace Neofilia.Domain;
//Table is an entity, must be accessed only inside Local aggregate boundry

//Table may own one reward
//Table may have many partecipants
// ?? Table may have many local games ?? not now, maybe future feature

//current idea: table is a lobby it self, when partecipants gain points on the runing challenge they add score points to the table lobby,
//when a threshold is reached the table creates a reward that can be redeemed, table can have only 1 reward at a time)

//TODO: add entity base class and inherit from it
public class Table : IEquatable<Table>
{
    private static readonly int _scoreCap = 100;
    private readonly List<Partecipant> _partecipants = [];
    private int _currentScore = 0;
    private Table() { } //ef ctor
    public readonly record struct TableId(int Value);
    public TableId Id { get; private set; } //PK
    public LocalId LocalId { get; private set; } //FK: Locals{ID}, REQUIRED

    //Probably will let a background service hook into this
    public event EventHandler<RewardGeneratedEvent>? RewardGenerated;
    public int TableNumber { get; private set; } //should this match the id?

    [NotMapped]
    public int TableScore 
    { get => _currentScore;
      private set
        {
            _currentScore += value;
            if (_currentScore >= _scoreCap)
            {
                GenerateReward();
                _currentScore = 0;
            }
        }
    }

    //navigation
    public Reward? Reward { get; private set; }
    public IReadOnlyCollection<Partecipant> Partecipants => _partecipants.AsReadOnly();

    public Table(LocalId localId, int tableNumber) => 
        (LocalId, TableNumber) = (localId, tableNumber);
    public void AddPartecipant(Partecipant user) => _partecipants.Add(user);
    public void RemovePartecipant(Partecipant user) => _partecipants.Remove(user);
    //TODO: implement socre system
    public void AddScore() => TableScore++;
    private void GenerateReward()
    {
        //TODO: implement reward system
        Reward = new Reward(RewardType.Drink, this.Id);
        OnRewardGenerated(new RewardGeneratedEvent(this.Id, Reward));
    }
    private void OnRewardGenerated(RewardGeneratedEvent e) => RewardGenerated?.Invoke(this, e);

    //TODO: move this into a generic base entity class
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

public class RewardGeneratedEvent(TableId tableId, Reward reward) : EventArgs
{
    public TableId Id {  get; init; } = tableId;
    public Reward Reward { get; init; } = reward;
}