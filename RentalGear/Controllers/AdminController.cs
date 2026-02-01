using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalGear.Data;
using RentalGear.Models;
using RentalGear.Models.ViewModels;

namespace RentalGear.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public AdminController(ApplicationDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var msc = DateTime.Now.AddMonths(-1);
        var model = new AdminDashboardVM
        {
            LiczbaSprzetu = await _db.Sprzety.CountAsync(s => s.Dostepny),
            LiczbaKategorii = await _db.Kategorie.CountAsync(k => k.Aktywna),
            WszystkieWypozyczenia = await _db.Wypozyczenia.CountAsync(),
            Oczekujace = await _db.Wypozyczenia.CountAsync(w => w.Status == StatusWypozyczenia.Oczekujace),
            WTrakcie = await _db.Wypozyczenia.CountAsync(w => w.Status == StatusWypozyczenia.WTrakcie),
            LiczbaKlientow = await _userManager.Users.CountAsync(),
            PrzychodMiesiac = await _db.Wypozyczenia.Where(w => w.DataUtworzenia >= msc && (w.Status == StatusWypozyczenia.Zakonczone || w.Status == StatusWypozyczenia.WTrakcie)).SumAsync(w => w.KosztCalkowity),
            PrzychodCalkowity = await _db.Wypozyczenia.Where(w => w.Status == StatusWypozyczenia.Zakonczone || w.Status == StatusWypozyczenia.WTrakcie).SumAsync(w => w.KosztCalkowity),
            OczekujaceWypozyczenia = await _db.Wypozyczenia.Where(w => w.Status == StatusWypozyczenia.Oczekujace).Include(w => w.Uzytkownik).Include(w => w.Pozycje).ThenInclude(p => p.Sprzet).OrderBy(w => w.DataOd).Take(10).ToListAsync(),
            OstatnieWypozyczenia = await _db.Wypozyczenia.Include(w => w.Uzytkownik).OrderByDescending(w => w.DataUtworzenia).Take(5).ToListAsync()
        };
        return View(model);
    }

    // ==================== ZAMÓWIENIA ====================

    public async Task<IActionResult> Zamowienia(StatusWypozyczenia? status)
    {
        var q = _db.Wypozyczenia.Include(w => w.Uzytkownik).Include(w => w.Pozycje).ThenInclude(p => p.Sprzet).AsQueryable();
        if (status.HasValue) q = q.Where(w => w.Status == status.Value);
        ViewBag.Status = status;
        return View(await q.OrderByDescending(w => w.DataUtworzenia).ToListAsync());
    }

    public async Task<IActionResult> ZamowienieSzczegoly(int id)
    {
        var w = await _db.Wypozyczenia.Include(w => w.Uzytkownik).Include(w => w.Pozycje).ThenInclude(p => p.Sprzet).FirstOrDefaultAsync(w => w.Id == id);
        return w == null ? NotFound() : View(w);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ZmienStatus(int id, StatusWypozyczenia status, string? uwagi)
    {
        var w = await _db.Wypozyczenia.FindAsync(id);
        if (w == null) return NotFound();
        w.Status = status;
        w.UwagiPracownika = uwagi;
        w.DataModyfikacji = DateTime.Now;
        await _db.SaveChangesAsync();
        TempData["Sukces"] = "Status zmieniony";
        return RedirectToAction("Zamowienia");
    }

    // ==================== SPRZĘT ====================

    public async Task<IActionResult> Sprzet() => View(await _db.Sprzety.Include(s => s.Kategoria).OrderBy(s => s.Kategoria!.Kolejnosc).ThenBy(s => s.Nazwa).ToListAsync());

    public async Task<IActionResult> DodajSprzet() => View(new EquipmentFormVM { Kategorie = new SelectList(await _db.Kategorie.Where(k => k.Aktywna).ToListAsync(), "Id", "Nazwa") });

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DodajSprzet(EquipmentFormVM m)
    {
        if (ModelState.IsValid)
        {
            _db.Sprzety.Add(new Sprzet
            {
                Nazwa = m.Nazwa, Producent = m.Producent, Model = m.Model, Opis = m.Opis,
                CenaDzien = m.CenaDzien, Kaucja = m.Kaucja, IloscDostepna = m.IloscCalkowita,
                IloscCalkowita = m.IloscCalkowita, ZdjecieUrl = m.ZdjecieUrl, Dostepny = m.Dostepny,
                Wyrozniony = m.Wyrozniony, Nowosc = m.Nowosc, KategoriaId = m.KategoriaId
            });
            await _db.SaveChangesAsync();
            TempData["Sukces"] = "Dodano sprzęt";
            return RedirectToAction("Sprzet");
        }
        m.Kategorie = new SelectList(await _db.Kategorie.Where(k => k.Aktywna).ToListAsync(), "Id", "Nazwa");
        return View(m);
    }

    public async Task<IActionResult> EdytujSprzet(int id)
    {
        var s = await _db.Sprzety.FindAsync(id);
        if (s == null) return NotFound();
        return View(new EquipmentFormVM
        {
            Id = s.Id, Nazwa = s.Nazwa, Producent = s.Producent, Model = s.Model, Opis = s.Opis,
            CenaDzien = s.CenaDzien, Kaucja = s.Kaucja, IloscCalkowita = s.IloscCalkowita,
            ZdjecieUrl = s.ZdjecieUrl, Dostepny = s.Dostepny, Wyrozniony = s.Wyrozniony,
            Nowosc = s.Nowosc, KategoriaId = s.KategoriaId,
            Kategorie = new SelectList(await _db.Kategorie.Where(k => k.Aktywna).ToListAsync(), "Id", "Nazwa")
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EdytujSprzet(int id, EquipmentFormVM m)
    {
        if (id != m.Id) return NotFound();
        if (ModelState.IsValid)
        {
            var s = await _db.Sprzety.FindAsync(id);
            if (s == null) return NotFound();
            s.Nazwa = m.Nazwa; s.Producent = m.Producent; s.Model = m.Model; s.Opis = m.Opis;
            s.CenaDzien = m.CenaDzien; s.Kaucja = m.Kaucja; s.IloscCalkowita = m.IloscCalkowita;
            s.ZdjecieUrl = m.ZdjecieUrl; s.Dostepny = m.Dostepny; s.Wyrozniony = m.Wyrozniony;
            s.Nowosc = m.Nowosc; s.KategoriaId = m.KategoriaId;
            await _db.SaveChangesAsync();
            TempData["Sukces"] = "Zaktualizowano sprzęt";
            return RedirectToAction("Sprzet");
        }
        m.Kategorie = new SelectList(await _db.Kategorie.Where(k => k.Aktywna).ToListAsync(), "Id", "Nazwa");
        return View(m);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UsunSprzet(int id)
    {
        var s = await _db.Sprzety.FindAsync(id);
        if (s != null) { s.Dostepny = false; await _db.SaveChangesAsync(); TempData["Sukces"] = "Dezaktywowano sprzęt"; }
        return RedirectToAction("Sprzet");
    }

    // ==================== UŻYTKOWNICY ====================

    public async Task<IActionResult> Uzytkownicy()
    {
        var users = await _userManager.Users.OrderBy(u => u.Nazwisko).ToListAsync();
        var roles = new Dictionary<string, IList<string>>();
        foreach (var u in users) roles[u.Id] = await _userManager.GetRolesAsync(u);
        ViewBag.Roles = roles;
        return View(users);
    }

    // ==================== KALENDARZ ====================

    public async Task<IActionResult> Kalendarz()
    {
        ViewBag.Sprzety = await _db.Sprzety.Where(s => s.Dostepny).ToListAsync();
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEvents(DateTime start, DateTime end)
    {
        var list = await _db.Wypozyczenia
            .Where(w => w.Status != StatusWypozyczenia.Anulowane && w.Status != StatusWypozyczenia.Odrzucone && w.DataOd <= end && w.DataDo >= start)
            .Include(w => w.Uzytkownik).Include(w => w.Pozycje).ThenInclude(p => p.Sprzet).ToListAsync();

        var events = list.Select(w => new
        {
            id = w.Id,
            title = $"{w.Uzytkownik?.PelnaNazwa} - {string.Join(", ", w.Pozycje.Select(p => p.Sprzet?.Nazwa).Take(2))}{(w.Pozycje.Count > 2 ? "..." : "")}",
            start = w.DataOd.ToString("yyyy-MM-dd"),
            end = w.DataDo.AddDays(1).ToString("yyyy-MM-dd"),
            color = w.Status switch { StatusWypozyczenia.Oczekujace => "#f59e0b", StatusWypozyczenia.Potwierdzone => "#3b82f6", StatusWypozyczenia.WTrakcie => "#8b5cf6", StatusWypozyczenia.Zakonczone => "#10b981", _ => "#6b7280" },
            url = Url.Action("ZamowienieSzczegoly", "Admin", new { id = w.Id })
        });
        return Json(events);
    }

    // ==================== KATEGORIE ====================

    public async Task<IActionResult> Kategorie() => View(await _db.Kategorie.OrderBy(k => k.Kolejnosc).ToListAsync());

    public IActionResult DodajKategorie() => View(new Kategoria());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DodajKategorie(Kategoria m)
    {
        if (ModelState.IsValid)
        {
            m.Kolejnosc = await _db.Kategorie.CountAsync() + 1;
            _db.Kategorie.Add(m);
            await _db.SaveChangesAsync();
            TempData["Sukces"] = "Dodano kategorię";
            return RedirectToAction("Kategorie");
        }
        return View(m);
    }

    public async Task<IActionResult> EdytujKategorie(int id)
    {
        var k = await _db.Kategorie.FindAsync(id);
        return k == null ? NotFound() : View(k);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EdytujKategorie(int id, Kategoria m)
    {
        if (id != m.Id) return NotFound();
        if (ModelState.IsValid)
        {
            var k = await _db.Kategorie.FindAsync(id);
            if (k == null) return NotFound();
            k.Nazwa = m.Nazwa;
            k.Ikona = m.Ikona;
            k.Opis = m.Opis;
            k.Kolejnosc = m.Kolejnosc;
            k.Aktywna = m.Aktywna;
            await _db.SaveChangesAsync();
            TempData["Sukces"] = "Zaktualizowano kategorię";
            return RedirectToAction("Kategorie");
        }
        return View(m);
    }

    // ==================== USTAWIENIA SKLEPU ====================

    public async Task<IActionResult> Ustawienia()
    {
        var ustawienia = await _db.UstawieniaSklep.FirstOrDefaultAsync();
        if (ustawienia == null)
        {
            ustawienia = new UstawieniaSklep();
            _db.UstawieniaSklep.Add(ustawienia);
            await _db.SaveChangesAsync();
        }
        return View(ustawienia);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Ustawienia(UstawieniaSklep m)
    {
        if (ModelState.IsValid)
        {
            var ustawienia = await _db.UstawieniaSklep.FirstOrDefaultAsync();
            if (ustawienia == null) return NotFound();

            ustawienia.NazwaSklepu = m.NazwaSklepu;
            ustawienia.OpisSklepu = m.OpisSklepu;
            ustawienia.Adres = m.Adres;
            ustawienia.Miasto = m.Miasto;
            ustawienia.KodPocztowy = m.KodPocztowy;
            ustawienia.Telefon = m.Telefon;
            ustawienia.Email = m.Email;
            ustawienia.NIP = m.NIP;
            ustawienia.GodzinyPonPt = m.GodzinyPonPt;
            ustawienia.GodzinySobota = m.GodzinySobota;
            ustawienia.GodzinyNiedziela = m.GodzinyNiedziela;
            ustawienia.MinDniWypozyczenia = m.MinDniWypozyczenia;
            ustawienia.MaxDniWypozyczenia = m.MaxDniWypozyczenia;
            ustawienia.DniWyprzedzenia = m.DniWyprzedzenia;
            ustawienia.RabatTydzien = m.RabatTydzien;
            ustawienia.RabatMiesiac = m.RabatMiesiac;
            ustawienia.DomyslnaKaucjaProcent = m.DomyslnaKaucjaProcent;
            ustawienia.MinimalnaKaucja = m.MinimalnaKaucja;
            ustawienia.OferujDostawe = m.OferujDostawe;
            ustawienia.KosztDostawy = m.KosztDostawy;
            ustawienia.DarmowaDostawaOd = m.DarmowaDostawaOd;
            ustawienia.EmailPowiadomien = m.EmailPowiadomien;
            ustawienia.PowiadomieniaNoweZamowienia = m.PowiadomieniaNoweZamowienia;
            ustawienia.PowiadomieniaAnulowania = m.PowiadomieniaAnulowania;
            ustawienia.FacebookUrl = m.FacebookUrl;
            ustawienia.InstagramUrl = m.InstagramUrl;
            ustawienia.YouTubeUrl = m.YouTubeUrl;
            ustawienia.Waluta = m.Waluta;
            ustawienia.SymbolWaluty = m.SymbolWaluty;
            ustawienia.DataModyfikacji = DateTime.Now;

            await _db.SaveChangesAsync();
            TempData["Sukces"] = "Ustawienia zapisane";
            return RedirectToAction("Ustawienia");
        }
        return View(m);
    }

    // ==================== RAPORTY ====================

    public async Task<IActionResult> Raporty()
    {
        var teraz = DateTime.Now;
        var poczatekMiesiaca = new DateTime(teraz.Year, teraz.Month, 1);
        var poczatekRoku = new DateTime(teraz.Year, 1, 1);

        // Przychody
        var przychodMiesiac = await _db.Wypozyczenia
            .Where(w => w.DataUtworzenia >= poczatekMiesiaca && (w.Status == StatusWypozyczenia.Zakonczone || w.Status == StatusWypozyczenia.WTrakcie))
            .SumAsync(w => w.KosztCalkowity);

        var przychodRok = await _db.Wypozyczenia
            .Where(w => w.DataUtworzenia >= poczatekRoku && (w.Status == StatusWypozyczenia.Zakonczone || w.Status == StatusWypozyczenia.WTrakcie))
            .SumAsync(w => w.KosztCalkowity);

        // Wypożyczenia wg statusu
        var wypozyczeniaWgStatusu = await _db.Wypozyczenia
            .GroupBy(w => w.Status)
            .Select(g => new { Status = g.Key, Liczba = g.Count() })
            .ToListAsync();

        // Najpopularniejszy sprzęt
        var popularnySprzet = await _db.PozycjeWypozyczen
            .GroupBy(p => p.SprzetId)
            .Select(g => new { SprzetId = g.Key, Liczba = g.Count() })
            .OrderByDescending(x => x.Liczba)
            .Take(10)
            .ToListAsync();

        var sprzetIds = popularnySprzet.Select(x => x.SprzetId).ToList();
        var sprzety = await _db.Sprzety.Where(s => sprzetIds.Contains(s.Id)).ToDictionaryAsync(s => s.Id);

        // Wypożyczenia miesięczne (ostatnie 6 miesięcy)
        var wypozyczeniaMiesieczne = new List<(string Miesiac, int Liczba, decimal Przychod)>();
        for (int i = 5; i >= 0; i--)
        {
            var miesiac = teraz.AddMonths(-i);
            var poczatek = new DateTime(miesiac.Year, miesiac.Month, 1);
            var koniec = poczatek.AddMonths(1);
            var liczba = await _db.Wypozyczenia.CountAsync(w => w.DataUtworzenia >= poczatek && w.DataUtworzenia < koniec);
            var przychod = await _db.Wypozyczenia
                .Where(w => w.DataUtworzenia >= poczatek && w.DataUtworzenia < koniec && (w.Status == StatusWypozyczenia.Zakonczone || w.Status == StatusWypozyczenia.WTrakcie))
                .SumAsync(w => w.KosztCalkowity);
            wypozyczeniaMiesieczne.Add((poczatek.ToString("MMM yyyy"), liczba, przychod));
        }

        ViewBag.PrzychodMiesiac = przychodMiesiac;
        ViewBag.PrzychodRok = przychodRok;
        ViewBag.WypozyczeniaWgStatusu = wypozyczeniaWgStatusu;
        ViewBag.PopularnySprzet = popularnySprzet.Select(x => new { Sprzet = sprzety.GetValueOrDefault(x.SprzetId), x.Liczba }).ToList();
        ViewBag.WypozyczeniaMiesieczne = wypozyczeniaMiesieczne;

        return View();
    }
}