using System.ComponentModel.DataAnnotations;

namespace RentalGear.Models;

public class Kategoria
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Nazwa { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Opis { get; set; }

    [StringLength(50)]
    public string Ikona { get; set; } = "bi-folder";

    public int Kolejnosc { get; set; }
    public bool Aktywna { get; set; } = true;

    public virtual ICollection<Sprzet> Sprzety { get; set; } = new List<Sprzet>();
}