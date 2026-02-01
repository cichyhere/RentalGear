using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalGear.Models;

public class PozycjaWypozyczenia
{
    public int Id { get; set; }
    public int Ilosc { get; set; } = 1;
    public decimal CenaDzien { get; set; }
    public decimal KosztPozycji { get; set; }

    [Required]
    public int WypozyczenieId { get; set; }

    [ForeignKey("WypozyczenieId")]
    public virtual Wypozyczenie? Wypozyczenie { get; set; }

    [Required]
    public int SprzetId { get; set; }

    [ForeignKey("SprzetId")]
    public virtual Sprzet? Sprzet { get; set; }
}