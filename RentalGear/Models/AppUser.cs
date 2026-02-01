using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RentalGear.Models;

public class AppUser : IdentityUser
{
    [Required, StringLength(50)]
    public string Imie { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Nazwisko { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Firma { get; set; }

    [StringLength(20)]
    public string? NIP { get; set; }

    [StringLength(200)]
    public string? Adres { get; set; }

    [StringLength(100)]
    public string? Miasto { get; set; }

    [StringLength(10)]
    public string? KodPocztowy { get; set; }

    public DateTime DataRejestracji { get; set; } = DateTime.Now;
    public bool Aktywny { get; set; } = true;

    public string PelnaNazwa => $"{Imie} {Nazwisko}";
    public string Inicjaly => $"{(Imie.Length > 0 ? Imie[0] : '?')}{(Nazwisko.Length > 0 ? Nazwisko[0] : '?')}";

    public virtual ICollection<Wypozyczenie> Wypozyczenia { get; set; } = new List<Wypozyczenie>();
}