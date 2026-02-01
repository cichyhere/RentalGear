using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using RentalGear.Data;
using RentalGear.Models;

namespace RentalGear.Controllers;

[Authorize]
public class WypozyczeniaController : Controller
{
    private readonly ApplicationDbContext _db;
    public WypozyczeniaController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index(StatusWypozyczenia? status)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var query = _db.Wypozyczenia
            .Where(w => w.UzytkownikId == userId)
            .Include(w => w.Pozycje).ThenInclude(p => p.Sprzet)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(w => w.Status == status.Value);

        ViewBag.Status = status;
        return View(await query.OrderByDescending(w => w.DataUtworzenia).ToListAsync());
    }

    public async Task<IActionResult> Szczegoly(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");

        var wypozyczenie = await _db.Wypozyczenia
            .Include(w => w.Pozycje).ThenInclude(p => p.Sprzet)
            .Include(w => w.Uzytkownik)
            .FirstOrDefaultAsync(w => w.Id == id && (w.UzytkownikId == userId || isAdmin));

        if (wypozyczenie == null) return NotFound();
        return View(wypozyczenie);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Anuluj(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var wypozyczenie = await _db.Wypozyczenia.FirstOrDefaultAsync(w => w.Id == id && w.UzytkownikId == userId);

        if (wypozyczenie == null) return NotFound();
        if (wypozyczenie.Status != StatusWypozyczenia.Oczekujace)
        {
            TempData["Blad"] = "Można anulować tylko oczekujące zamówienia";
            return RedirectToAction("Index");
        }

        wypozyczenie.Status = StatusWypozyczenia.Anulowane;
        wypozyczenie.DataModyfikacji = DateTime.Now;
        await _db.SaveChangesAsync();

        TempData["Sukces"] = "Zamówienie anulowane";
        return RedirectToAction("Index");
    }
}