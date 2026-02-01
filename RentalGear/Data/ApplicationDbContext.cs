using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentalGear.Models;

namespace RentalGear.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Kategoria> Kategorie => Set<Kategoria>();
    public DbSet<Sprzet> Sprzety => Set<Sprzet>();
    public DbSet<Wypozyczenie> Wypozyczenia => Set<Wypozyczenie>();
    public DbSet<PozycjaWypozyczenia> PozycjeWypozyczen => Set<PozycjaWypozyczenia>();
    public DbSet<UstawieniaSklep> UstawieniaSklep => Set<UstawieniaSklep>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Sprzet>()
            .HasOne(s => s.Kategoria)
            .WithMany(k => k.Sprzety)
            .HasForeignKey(s => s.KategoriaId);

        builder.Entity<Wypozyczenie>()
            .HasOne(w => w.Uzytkownik)
            .WithMany()
            .HasForeignKey(w => w.UzytkownikId);

        builder.Entity<PozycjaWypozyczenia>()
            .HasOne(p => p.Wypozyczenie)
            .WithMany(w => w.Pozycje)
            .HasForeignKey(p => p.WypozyczenieId);

        builder.Entity<PozycjaWypozyczenia>()
            .HasOne(p => p.Sprzet)
            .WithMany()
            .HasForeignKey(p => p.SprzetId);
    }
}