using static Neofilia.Domain.Table;

namespace Neofilia.Domain;

public class Partecipant
{
    public Guid Id { get; private set; }
    public TableId TableId { get; private set; }
    public NotEmptyString UserName { get; private set; }
}
