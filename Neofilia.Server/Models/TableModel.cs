using Neofilia.Domain;
using System.ComponentModel.DataAnnotations;

namespace Neofilia.Server.Models;

public class TableModel
{
    public int Id { get; set; }
    public int LocalId { get; set; }

    [Required(ErrorMessage = "TableNumber is required")]
    [Range(1, int.MaxValue, ErrorMessage = "TableNumber must be a positive number")]
    public int TableNumber { get; set; }
    public Table ToTable() =>
        new(new Local.LocalId(LocalId), TableNumber);
}
