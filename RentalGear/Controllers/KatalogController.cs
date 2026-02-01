using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalGear.Data;
using RentalGear.Models;

namespace RentalGear.Controllers;

public class KatalogController : Controller
{
    private readonly ApplicationDbContext _db;
    public KatalogController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? szukaj, int? kategoriaId, string? sortowanie)
    {
        ViewBag.Kategorie = await _db.Kategorie.Where(k => k.Aktywna).OrderBy(k => k.Kolejnosc).ToListAsync();

        var query = _db.Sprzety.Where(s => s.Dostepny).Include(s => s.Kategoria).AsQueryable();

        if (!string.IsNullOrEmpty(szukaj))
        {
            query = query.Where(s => s.Nazwa.Contains(szukaj) || s.Producent.Contains(szukaj));
            ViewBag.Szukaj = szukaj;
        }

        if (kategoriaId.HasValue)
        {
            query = query.Where(s => s.KategoriaId == kategoriaId);
            ViewBag.KategoriaId = kategoriaId;
            ViewBag.AktualnaKategoria = await _db.Kategorie.FindAsync(kategoriaId);
        }

        query = sortowanie switch
        {
            "cena_asc" => query.OrderBy(s => s.CenaDzien),
            "cena_desc" => query.OrderByDescending(s => s.CenaDzien),
            "nazwa" => query.OrderBy(s => s.Nazwa),
            "najnowsze" => query.OrderByDescending(s => s.DataDodania),
            _ => query.OrderBy(s => s.Kategoria!.Kolejnosc).ThenBy(s => s.Nazwa)
        };
        ViewBag.Sortowanie = sortowanie;

        return View(await query.ToListAsync());
    }

    public async Task<IActionResult> Szczegoly(int id)
    {
        var sprzet = await _db.Sprzety.Include(s => s.Kategoria).FirstOrDefaultAsync(s => s.Id == id);
        if (sprzet == null) return NotFound();

        sprzet.Wyswietlenia++;
        await _db.SaveChangesAsync();

        ViewBag.Podobne = await _db.Sprzety
            .Where(s => s.KategoriaId == sprzet.KategoriaId && s.Id != id && s.Dostepny)
            .Take(4).ToListAsync();

        return View(sprzet);
    }

    public async Task<IActionResult> Kategoria(int id)
    {
        var kategoria = await _db.Kategorie.FindAsync(id);
        if (kategoria == null) return NotFound();

        ViewBag.Kategoria = kategoria;
        ViewBag.Kategorie = await _db.Kategorie.Where(k => k.Aktywna).OrderBy(k => k.Kolejnosc).ToListAsync();

        var sprzety = await _db.Sprzety
            .Where(s => s.KategoriaId == id && s.Dostepny)
            .Include(s => s.Kategoria)
            .ToListAsync();

        return View(sprzety);
    }

    [HttpGet]
    public async Task<IActionResult> Kalendarz(int id)
    {
        var sprzet = await _db.Sprzety.FindAsync(id);
        if (sprzet == null) return NotFound();
        return View(sprzet);
    }

    [HttpGet]
    public async Task<IActionResult> GetKalendarzEvents(int id, DateTime start, DateTime end)
    {
        var pozycje = await _db.PozycjeWypozyczen
            .Include(p => p.Wypozyczenie)
            .Where(p => p.SprzetId == id)
            .ToListAsync();

        var filtered = pozycje.Where(p =>
            p.Wypozyczenie != null &&
            p.Wypozyczenie.Status != StatusWypozyczenia.Anulowane &&
            p.Wypozyczenie.Status != StatusWypozyczenia.Odrzucone &&
            p.Wypozyczenie.DataOd <= end &&
            p.Wypozyczenie.DataDo >= start);

        var events = filtered.Select(p => new
        {
            id = p.WypozyczenieId,
            title = p.Wypozyczenie!.Status == StatusWypozyczenia.Oczekujace ? "Oczekujące" : "Zarezerwowane",
            start = p.Wypozyczenie.DataOd.ToString("yyyy-MM-dd"),
            end = p.Wypozyczenie.DataDo.AddDays(1).ToString("yyyy-MM-dd"),
            color = p.Wypozyczenie.Status == StatusWypozyczenia.Oczekujace ? "#f59e0b" : "#ef4444",
            allDay = true
        }).ToList();

        return Json(events);
    }
}