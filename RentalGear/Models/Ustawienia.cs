using System.ComponentModel.DataAnnotations;

namespace RentalGear.Models;

public class UstawieniaSklep
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Nazwa sklepu")]
    public string NazwaSklepu { get; set; } = "RentalGear";

    [Display(Name = "Opis sklepu")]
    public string? OpisSklepu { get; set; }

    [Display(Name = "Adres")]
    public string? Adres { get; set; }

    [Display(Name = "Miasto")]
    public string? Miasto { get; set; }

    [Display(Name = "Kod pocztowy")]
    public string? KodPocztowy { get; set; }

    [Display(Name = "Telefon")]
    public string? Telefon { get; set; }

    [Display(Name = "Email kontaktowy")]
    [EmailAddress]
    public string? Email { get; set; }

    [Display(Name = "NIP")]
    public string? NIP { get; set; }

    // Godziny otwarcia
    [Display(Name = "Godziny otwarcia (Pon-Pt)")]
    public string? GodzinyPonPt { get; set; } = "9:00 - 18:00";

    [Display(Name = "Godziny otwarcia (Sobota)")]
    public string? GodzinySobota { get; set; } = "10:00 - 14:00";

    [Display(Name = "Godziny otwarcia (Niedziela)")]
    public string? GodzinyNiedziela { get; set; } = "Zamknięte";

    // Ustawienia wypożyczeń
    [Display(Name = "Minimalna liczba dni wypożyczenia")]
    [Range(1, 30)]
    public int MinDniWypozyczenia { get; set; } = 1;

    [Display(Name = "Maksymalna liczba dni wypożyczenia")]
    [Range(1, 365)]
    public int MaxDniWypozyczenia { get; set; } = 30;

    [Display(Name = "Dni wyprzedzenia przy rezerwacji")]
    [Range(0, 30)]
    public int DniWyprzedzenia { get; set; } = 1;

    [Display(Name = "Rabat za tydzień (%)")]
    [Range(0, 50)]
    public decimal RabatTydzien { get; set; } = 10;

    [Display(Name = "Rabat za miesiąc (%)")]
    [Range(0, 70)]
    public decimal RabatMiesiac { get; set; } = 20;

    // Kaucja
    [Display(Name = "Domyślna kaucja (% wartości sprzętu)")]
    [Range(0, 100)]
    public decimal DomyslnaKaucjaProcent { get; set; } = 30;

    [Display(Name = "Minimalna kaucja (zł)")]
    [Range(0, 10000)]
    public decimal MinimalnaKaucja { get; set; } = 100;

    // Dostawa
    [Display(Name = "Oferuj dostawę")]
    public bool OferujDostawe { get; set; } = true;

    [Display(Name = "Koszt dostawy (zł)")]
    [Range(0, 500)]
    public decimal KosztDostawy { get; set; } = 50;

    [Display(Name = "Darmowa dostawa od kwoty (zł)")]
    [Range(0, 10000)]
    public decimal DarmowaDostawaOd { get; set; } = 500;

    // Powiadomienia
    [Display(Name = "Email do powiadomień")]
    [EmailAddress]
    public string? EmailPowiadomien { get; set; }

    [Display(Name = "Powiadomienia o nowych zamówieniach")]
    public bool PowiadomieniaNoweZamowienia { get; set; } = true;

    [Display(Name = "Powiadomienia o anulowaniach")]
    public bool PowiadomieniaAnulowania { get; set; } = true;

    // Social Media
    [Display(Name = "Facebook URL")]
    public string? FacebookUrl { get; set; }

    [Display(Name = "Instagram URL")]
    public string? InstagramUrl { get; set; }

    [Display(Name = "YouTube URL")]
    public string? YouTubeUrl { get; set; }

    // Inne
    [Display(Name = "Regulamin (HTML)")]
    public string? RegulaminHtml { get; set; }

    [Display(Name = "Polityka prywatności (HTML)")]
    public string? PolitykaPrywatnosciHtml { get; set; }

    [Display(Name = "Waluta")]
    public string Waluta { get; set; } = "PLN";

    [Display(Name = "Symbol waluty")]
    public string SymbolWaluty { get; set; } = "zł";

    public DateTime DataModyfikacji { get; set; } = DateTime.Now;
}