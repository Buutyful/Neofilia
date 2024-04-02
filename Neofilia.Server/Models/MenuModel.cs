using Neofilia.Domain;
using System.ComponentModel.DataAnnotations;

namespace Neofilia.Server.Models;

public class MenuModel
{
    public int Id { get; set; }
    public int LocalId { get; set; }

    [Url(ErrorMessage = "Url must be a valid link")]
    public Uri? Url { get; set; }

    public MenuModel(int id, int localId, string url)
    {
        Id = id;
        LocalId = localId;
        Uri.TryCreate(url, UriKind.Absolute, out var res);
        Url = res;
    }
    public Menu ToMenu() =>
        new(Url, new Local.LocalId(LocalId));
   
}
