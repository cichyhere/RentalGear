using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentalGear.Models;

namespace RentalGear.Models.ViewModels;

public class LoginVM
{
    [Required(ErrorMessage = "Email wymagany")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Hasło wymagane")]
    [DataType(DataType.Password)]
    public string Haslo { get; set; } = string.Empty;

    public bool Zapamietaj { get; set; }
}

public class RegisterVM
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Imie { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Nazwisko { get; set; } = string.Empty;

    public string? Telefon { get; set; }
    public string? Firma { get; set; }

    [Required, StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Haslo { get; set; } = string.Empty;

    [Required, Compare("Haslo")]
    [DataType(DataType.Password)]
    public string PotwierdzHaslo { get; set; } = string.Empty;
}

public class CartItemVM
{
    public int SprzetId { get; set; }
    public string Nazwa { get; set; } = string.Empty;
    public string? ZdjecieUrl { get; set; }
    public decimal CenaDzien { get; set; }
    public decimal Kaucja { get; set; }
    public int Ilosc { get; set; } = 1;
    public int MaxIlosc { get; set; }
}

public class OrderVM
{
    [Required]
    public DateTime DataOd { get; set; } = DateTime.Today.AddDays(1);

    [Required]
    public DateTime DataDo { get; set; } = DateTime.Today.AddDays(2);

    public string? Uwagi { get; set; }
    public List<CartItemVM> Koszyk { get; set; } = new();
}

public class EquipmentFormVM
{
    public int Id { get; set; }

    [Required]
    public string Nazwa { get; set; } = string.Empty;

    [Required]
    public string Producent { get; set; } = string.Empty;

    public string? Model { get; set; }
    public string? Opis { get; set; }

    [Required]
    public decimal CenaDzien { get; set; }

    public decimal? Kaucja { get; set; }
    public int IloscCalkowita { get; set; } = 1;
    public string? ZdjecieUrl { get; set; }
    public bool Dostepny { get; set; } = true;
    public bool Wyrozniony { get; set; }
    public bool Nowosc { get; set; }

    [Required]
    public int KategoriaId { get; set; }

    public SelectList? Kategorie { get; set; }
}

public class AdminDashboardVM
{
    public int LiczbaSprzetu { get; set; }
    public int LiczbaKategorii { get; set; }
    public int WszystkieWypozyczenia { get; set; }
    public int Oczekujace { get; set; }
    public int WTrakcie { get; set; }
    public int LiczbaKlientow { get; set; }
    public decimal PrzychodMiesiac { get; set; }
    public decimal PrzychodCalkowity { get; set; }
    public List<Wypozyczenie> OstatnieWypozyczenia { get; set; } = new();
    public List<Wypozyczenie> OczekujaceWypozyczenia { get; set; } = new();
}