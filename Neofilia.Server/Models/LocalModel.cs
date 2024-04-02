using System.ComponentModel.DataAnnotations;

namespace Neofilia.Server.Models;

public class LocalModel
{
    public int Id { get; set; }  
    public string ApplicationUserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Street is required")]
    [MaxLength(255, ErrorMessage = "Street cannot exceed 255 characters")]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "CivilNumber is required")]
    [MaxLength(255, ErrorMessage = "CivilNumber cannot exceed 255 characters")]
    public string CivilNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "PhoneNumber is required")]
    [MaxLength(255, ErrorMessage = "PhoneNumber cannot exceed 255 characters")]
    [RegularExpression(@"^\+39\d{9,10}$", ErrorMessage = "PhoneNumber must be in Italian format")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "EventStartsAt is required")]
    public DateTimeOffset EventStartsAt { get; set; }

    [Required(ErrorMessage = "EventEndsAt is required")]
    public DateTimeOffset EventEndsAt { get; set; }

}
