using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using RentalGear.Data;
using RentalGear.Models;
using RentalGear.Models.ViewModels;

namespace RentalGear.Controllers;

[Authorize]
public class KoszykController : Controller
{
    private readonly ApplicationDbContext _db;
    private const string CartKey = "Koszyk";

    public KoszykController(ApplicationDbContext db) => _db = db;

    public IActionResult Index() => View(GetCart());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Dodaj(int id, int ilosc = 1)
    {
        var sprzet = await _db.Sprzety.FindAsync(id);
        if (sprzet == null || !sprzet.Dostepny)
        {
            TempData["Blad"] = "Sprzęt niedostępny";
            return RedirectToAction("Index", "Katalog");
        }

        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.SprzetId == id);

        if (item != null)
            item.Ilosc = Math.Min(item.Ilosc + ilosc, sprzet.IloscDostepna);
        else
            cart.Add(new CartItemVM
            {
                SprzetId = sprzet.Id,
                Nazwa = $"{sprzet.Producent} {sprzet.Nazwa}",
                ZdjecieUrl = sprzet.ZdjecieUrl,
                CenaDzien = sprzet.CenaDzien,
                Kaucja = sprzet.Kaucja ?? 0,
                Ilosc = Math.Min(ilosc, sprzet.IloscDostepna),
                MaxIlosc = sprzet.IloscDostepna
            });

        SaveCart(cart);
        TempData["Sukces"] = $"Dodano {sprzet.Nazwa}";
        return RedirectToAction("Index");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Aktualizuj(int id, int ilosc)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.SprzetId == id);
        if (item != null)
        {
            if (ilosc <= 0) cart.Remove(item);
            else item.Ilosc = Math.Min(ilosc, item.MaxIlosc);
            SaveCart(cart);
        }
        return RedirectToAction("Index");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Usun(int id)
    {
        var cart = GetCart();
        cart.RemoveAll(c => c.SprzetId == id);
        SaveCart(cart);
        TempData["Sukces"] = "Usunięto";
        return RedirectToAction("Index");
    }

    public IActionResult Zamowienie()
    {
        var cart = GetCart();
        if (!cart.Any())
        {
            TempData["Blad"] = "Koszyk pusty";
            return RedirectToAction("Index");
        }
        return View(new OrderVM { Koszyk = cart });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Zamowienie(OrderVM model)
    {
        var cart = GetCart();
        model.Koszyk = cart;

        if (!cart.Any())
        {
            TempData["Blad"] = "Koszyk pusty";
            return RedirectToAction("Index");
        }

        if (model.DataDo < model.DataOd)
        {
            ModelState.AddModelError("DataDo", "Nieprawidłowa data");
            return View(model);
        }

        var dni = Math.Max(1, (model.DataDo - model.DataOd).Days + 1);
        decimal koszt = 0, kaucja = 0;
        foreach (var c in cart)
        {
            koszt += c.CenaDzien * c.Ilosc * dni;
            kaucja += c.Kaucja * c.Ilosc;
        }

        var wyp = new Wypozyczenie
        {
            DataOd = model.DataOd,
            DataDo = model.DataDo,
            UwagiKlienta = model.Uwagi,
            KosztCalkowity = koszt,
            Kaucja = kaucja,
            UzytkownikId = User.FindFirstValue(ClaimTypes.NameIdentifier)!
        };

        _db.Wypozyczenia.Add(wyp);
        await _db.SaveChangesAsync();

        foreach (var c in cart)
        {
            _db.PozycjeWypozyczen.Add(new PozycjaWypozyczenia
            {
                WypozyczenieId = wyp.Id,
                SprzetId = c.SprzetId,
                Ilosc = c.Ilosc,
                CenaDzien = c.CenaDzien,
                KosztPozycji = c.CenaDzien * c.Ilosc * dni
            });
        }
        await _db.SaveChangesAsync();

        HttpContext.Session.Remove(CartKey);
        TempData["Sukces"] = "Zamówienie złożone!";
        return RedirectToAction("Szczegoly", "Wypozyczenia", new { id = wyp.Id });
    }

    [HttpGet]
    public IActionResult ObliczKoszt(DateTime dataOd, DateTime dataDo)
    {
        var cart = GetCart();
        var dni = Math.Max(1, (dataDo - dataOd).Days + 1);
        decimal koszt = 0, kaucja = 0;
        foreach (var c in cart)
        {
            koszt += c.CenaDzien * c.Ilosc * dni;
            kaucja += c.Kaucja * c.Ilosc;
        }
        return Json(new { dni, koszt, kaucja, razem = koszt + kaucja });
    }

    [HttpGet]
    public IActionResult GetCount()
    {
        int count = 0;
        foreach (var c in GetCart()) count += c.Ilosc;
        return Json(new { count });
    }

    private List<CartItemVM> GetCart()
    {
        var json = HttpContext.Session.GetString(CartKey);
        return string.IsNullOrEmpty(json) ? new() : JsonSerializer.Deserialize<List<CartItemVM>>(json) ?? new();
    }

    private void SaveCart(List<CartItemVM> cart) => HttpContext.Session.SetString(CartKey, JsonSerializer.Serialize(cart));
}