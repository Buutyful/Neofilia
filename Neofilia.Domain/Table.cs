using static Neofilia.Domain.Locale;

namespace Neofilia.Domain;

//Table must have a global Locale game
//Table can have many local games
//Table can have many partecipants : how to implement this? table should host a lobby?

public class Table
{
    public record struct TableId(int Id);
    public TableId Id { get; private set; }
    public LocaleId LocaleId { get; private set; }
    public int TableNumber { get; private set; } //should this match the id?
}
