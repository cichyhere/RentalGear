using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalGear.Models;

public class Sprzet
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string Nazwa { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Producent { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Model { get; set; }

    [StringLength(2000)]
    public string? Opis { get; set; }

    [Required]
    public decimal CenaDzien { get; set; }

    public decimal? Kaucja { get; set; }
    public int IloscDostepna { get; set; } = 1;
    public int IloscCalkowita { get; set; } = 1;

    [StringLength(500)]
    public string? ZdjecieUrl { get; set; }

    public bool Dostepny { get; set; } = true;
    public bool Wyrozniony { get; set; }
    public bool Nowosc { get; set; }
    public int Wyswietlenia { get; set; }
    public DateTime DataDodania { get; set; } = DateTime.Now;

    [Required]
    public int KategoriaId { get; set; }

    [ForeignKey("KategoriaId")]
    public virtual Kategoria? Kategoria { get; set; }

    public virtual ICollection<PozycjaWypozyczenia> PozycjeWypozyczen { get; set; } = new List<PozycjaWypozyczenia>();
}