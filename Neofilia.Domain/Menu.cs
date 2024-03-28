namespace Neofilia.Domain;

//Not defined yet, could link to external menu resource or be implemented directly
public class Menu
{
    private Menu() { } //ef ctor
    public readonly record struct MenuId(int Id);
    public MenuId Id { get; private set; }
    public Uri? Url { get; private set; }

    public Menu(Uri? url) => Url = url;

    private Menu(MenuId id, Uri? url)
    {
        Id = id;
        Url = url;
    }
    public static Menu CreateTestMenu(MenuId id, Uri? url) => 
        new(id, url);
}
