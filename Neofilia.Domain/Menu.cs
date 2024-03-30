using static Neofilia.Domain.Local;

namespace Neofilia.Domain;
//Owned by Local
//Not defined yet, could link to external menu resource or be implemented directly
public class Menu
{
    private Menu() { } //ef ctor
    public readonly record struct MenuId(int Id);
    public MenuId Id { get; private set; }//PK
    public LocalId LocalId { get; private set; } //FK: Locals{Id}
    public Uri? Url { get; private set; }

    public Menu(Uri? url, LocalId localId) =>
        (Url, LocalId) = (url, localId);

    private Menu(MenuId id, Uri? url)
    {
        Id = id;
        Url = url;
    }
    public static Menu CreateTestMenu(MenuId id, Uri? url) => 
        new(id, url);
}
