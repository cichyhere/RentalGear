# 🎬 RentalGear - Wypożyczalnia Sprzętu Foto-Video

## 🛠️ Technologie
- **Backend:** ASP.NET Core 8.0 MVC
- **Baza danych:** SQLite + Entity Framework Core
- **Autoryzacja:** ASP.NET Core Identity
- **Frontend:** Bootstrap 5.3, Bootstrap Icons
- **Kalendarz:** FullCalendar 6.1

## 🚀 Szybki start

### Wymagania
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Instalacja

```bash
# Klonowanie repozytorium
git clone https://github.com/username/RentalGear.git
cd RentalGear

# Uruchomienie aplikacji
dotnet run
```
### Dane testowe

Przy pierwszym uruchomieniu aplikacja automatycznie utworzy bazę danych z przykładowymi danymi.



## 📊 Diagram bazy danych

```
AspNetUsers (AppUser)
    │
    │ 1:N
    ▼
Wypozyczenie ◄─── 1:N ───► PozycjaWypozyczenia
                                    │
                                    │ N:1
                                    ▼
Kategoria ◄─── 1:N ───► Sprzet
```


