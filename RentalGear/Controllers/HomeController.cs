using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalGear.Data;

namespace RentalGear.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    public HomeController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewBag.Kategorie = await _db.Kategorie.Where(k => k.Aktywna).OrderBy(k => k.Kolejnosc).ToListAsync();
        ViewBag.Wyroznione = await _db.Sprzety.Where(s => s.Dostepny && s.Wyrozniony).Include(s => s.Kategoria).Take(8).ToListAsync();
        ViewBag.Nowosci = await _db.Sprzety.Where(s => s.Dostepny && s.Nowosc).Include(s => s.Kategoria).Take(4).ToListAsync();
        ViewBag.LiczbaSprzetu = await _db.Sprzety.CountAsync(s => s.Dostepny);
        return View();
    }

    public IActionResult Kontakt() => View();
    public IActionResult Regulamin() => View();
    public IActionResult FAQ() => View();
    public IActionResult JakToDziala() => View();
}