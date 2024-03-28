namespace Neofilia.Domain;

//Not defined yet, could link to external menu resource or be implemented directly
public class Menu
{
    private Menu() { } //ef ctor
    public readonly record struct MenuId(int Id);
    public MenuId Id { get; private set; }
    public Uri? Url { get; private set; }
}
