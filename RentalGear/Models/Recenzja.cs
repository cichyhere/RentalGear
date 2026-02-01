using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalGear.Models;

public class Recenzja
{
    public int Id { get; set; }

    [Required]
    [Range(1, 5)]
    public int Ocena { get; set; }

    [StringLength(1000)]
    public string? Komentarz { get; set; }

    public DateTime DataDodania { get; set; } = DateTime.Now;

    public bool Zatwierdzona { get; set; } = false;

    [Required]
    public string UzytkownikId { get; set; } = string.Empty;

    [ForeignKey("UzytkownikId")]
    public virtual AppUser? Uzytkownik { get; set; }

    [Required]
    public int SprzetId { get; set; }

    [ForeignKey("SprzetId")]
    public virtual Sprzet? Sprzet { get; set; }
}