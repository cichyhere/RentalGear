using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalGear.Models;

public class Ulubione
{
    public int Id { get; set; }

    [Required]
    public string UzytkownikId { get; set; } = string.Empty;

    [ForeignKey("UzytkownikId")]
    public virtual AppUser? Uzytkownik { get; set; }

    [Required]
    public int SprzetId { get; set; }

    [ForeignKey("SprzetId")]
    public virtual Sprzet? Sprzet { get; set; }

    public DateTime DataDodania { get; set; } = DateTime.Now;
}