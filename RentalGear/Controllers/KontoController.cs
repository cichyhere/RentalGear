using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentalGear.Models;
using RentalGear.Models.ViewModels;

namespace RentalGear.Controllers;

public class KontoController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public KontoController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Logowanie(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginVM());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logowanie(LoginVM model, string? returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Haslo, model.Zapamietaj, false);
            if (result.Succeeded)
                return LocalRedirect(returnUrl ?? "/");
            ModelState.AddModelError("", "Nieprawidłowy email lub hasło");
        }
        return View(model);
    }

    public IActionResult Rejestracja() => View(new RegisterVM());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Rejestracja(RegisterVM model)
    {
        if (ModelState.IsValid)
        {
            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                Imie = model.Imie,
                Nazwisko = model.Nazwisko,
                PhoneNumber = model.Telefon,
                Firma = model.Firma,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Haslo);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Klient");
                await _signInManager.SignInAsync(user, false);
                TempData["Sukces"] = "Konto utworzone!";
                return RedirectToAction("Index", "Home");
            }
            foreach (var e in result.Errors)
                ModelState.AddModelError("", e.Description);
        }
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Wyloguj()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult BrakDostepu() => View();
}