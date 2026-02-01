using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalGear.Models;

public enum TypPowiadomienia
{
    Info,
    Sukces,
    Ostrzezenie,
    Blad,
    Zamowienie
}

public class Powiadomienie
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Tytul { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Tresc { get; set; }

    public TypPowiadomienia Typ { get; set; } = TypPowiadomienia.Info;

    public bool Przeczytane { get; set; } = false;

    public DateTime DataUtworzenia { get; set; } = DateTime.Now;

    [StringLength(200)]
    public string? LinkUrl { get; set; }

    [Required]
    public string UzytkownikId { get; set; } = string.Empty;

    [ForeignKey("UzytkownikId")]
    public virtual AppUser? Uzytkownik { get; set; }
}