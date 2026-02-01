using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalGear.Models;

public enum StatusWypozyczenia
{
    Oczekujace, Potwierdzone, WTrakcie, Zakonczone, Anulowane, Odrzucone
}

public class Wypozyczenie
{
    public int Id { get; set; }

    [Required]
    public DateTime DataOd { get; set; }

    [Required]
    public DateTime DataDo { get; set; }

    public StatusWypozyczenia Status { get; set; } = StatusWypozyczenia.Oczekujace;

    [StringLength(1000)]
    public string? UwagiKlienta { get; set; }

    [StringLength(1000)]
    public string? UwagiPracownika { get; set; }

    public decimal KosztCalkowity { get; set; }
    public decimal Kaucja { get; set; }
    public DateTime DataUtworzenia { get; set; } = DateTime.Now;
    public DateTime? DataModyfikacji { get; set; }

    [Required]
    public string UzytkownikId { get; set; } = string.Empty;

    [ForeignKey("UzytkownikId")]
    public virtual AppUser? Uzytkownik { get; set; }

    public virtual ICollection<PozycjaWypozyczenia> Pozycje { get; set; } = new List<PozycjaWypozyczenia>();

    public int LiczbaDni => Math.Max(1, (DataDo - DataOd).Days + 1);
}