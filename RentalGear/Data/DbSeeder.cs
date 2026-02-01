using Microsoft.AspNetCore.Identity;
using RentalGear.Models;

namespace RentalGear.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider sp)
    {
        var context = sp.GetRequiredService<ApplicationDbContext>();
        var userManager = sp.GetRequiredService<UserManager<AppUser>>();
        var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.EnsureCreatedAsync();

        // Roles
        foreach (var role in new[] { "Admin", "Klient" })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // Admin
        if (await userManager.FindByEmailAsync("admin@rentalgear.pl") == null)
        {
            var admin = new AppUser
            {
                UserName = "admin@rentalgear.pl",
                Email = "admin@rentalgear.pl",
                Imie = "Admin",
                Nazwisko = "System",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        // Klient 1
        AppUser? jan = await userManager.FindByEmailAsync("klient@test.pl");
        if (jan == null)
        {
            jan = new AppUser
            {
                UserName = "klient@test.pl",
                Email = "klient@test.pl",
                Imie = "Jan",
                Nazwisko = "Kowalski",
                Firma = "Studio Video Pro",
                PhoneNumber = "+48 123 456 789",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(jan, "Klient123!");
            await userManager.AddToRoleAsync(jan, "Klient");
        }

        // Klient 2
        AppUser? anna = await userManager.FindByEmailAsync("anna@filmstudio.pl");
        if (anna == null)
        {
            anna = new AppUser
            {
                UserName = "anna@filmstudio.pl",
                Email = "anna@filmstudio.pl",
                Imie = "Anna",
                Nazwisko = "Nowak",
                Firma = "Film Studio Warszawa",
                PhoneNumber = "+48 987 654 321",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(anna, "Klient123!");
            await userManager.AddToRoleAsync(anna, "Klient");
        }

        // Klient 3
        AppUser? piotr = await userManager.FindByEmailAsync("piotr@weddings.pl");
        if (piotr == null)
        {
            piotr = new AppUser
            {
                UserName = "piotr@weddings.pl",
                Email = "piotr@weddings.pl",
                Imie = "Piotr",
                Nazwisko = "Wiśniewski",
                Firma = "Weddings & Events",
                PhoneNumber = "+48 555 123 456",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(piotr, "Klient123!");
            await userManager.AddToRoleAsync(piotr, "Klient");
        }

        // Klient 4
        AppUser? marta = await userManager.FindByEmailAsync("marta@docs.pl");
        if (marta == null)
        {
            marta = new AppUser
            {
                UserName = "marta@docs.pl",
                Email = "marta@docs.pl",
                Imie = "Marta",
                Nazwisko = "Zielińska",
                Firma = "Documentary Films",
                PhoneNumber = "+48 666 789 012",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(marta, "Klient123!");
            await userManager.AddToRoleAsync(marta, "Klient");
        }

        // Klient 5
        AppUser? tomek = await userManager.FindByEmailAsync("tomek@agencja.pl");
        if (tomek == null)
        {
            tomek = new AppUser
            {
                UserName = "tomek@agencja.pl",
                Email = "tomek@agencja.pl",
                Imie = "Tomasz",
                Nazwisko = "Adamski",
                Firma = "Agencja Reklamowa XYZ",
                PhoneNumber = "+48 777 888 999",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(tomek, "Klient123!");
            await userManager.AddToRoleAsync(tomek, "Klient");
        }

        // Kategorie
        if (!context.Kategorie.Any())
        {
            context.Kategorie.AddRange(new List<Kategoria>
            {
                new() { Nazwa = "Kamery", Ikona = "bi-camera-video-fill", Kolejnosc = 1 },
                new() { Nazwa = "Aparaty", Ikona = "bi-camera-fill", Kolejnosc = 2 },
                new() { Nazwa = "Obiektywy", Ikona = "bi-circle", Kolejnosc = 3 },
                new() { Nazwa = "Oświetlenie", Ikona = "bi-lightbulb-fill", Kolejnosc = 4 },
                new() { Nazwa = "Statywy i Gimble", Ikona = "bi-easel-fill", Kolejnosc = 5 },
                new() { Nazwa = "Audio", Ikona = "bi-mic-fill", Kolejnosc = 6 },
                new() { Nazwa = "Drony", Ikona = "bi-airplane-fill", Kolejnosc = 7 },
                new() { Nazwa = "Akcesoria", Ikona = "bi-bag-fill", Kolejnosc = 8 }
            });
            await context.SaveChangesAsync();
        }

        // Sprzęt
        if (!context.Sprzety.Any())
        {
            context.Sprzety.AddRange(new List<Sprzet>
            {
                // KAMERY (1) - ID: 1-4
                new() { Nazwa = "FX6 Full-Frame Cinema Camera", Producent = "Sony", CenaDzien = 450, Kaucja = 5000, IloscDostepna = 2, IloscCalkowita = 2, KategoriaId = 1, Wyrozniony = true,
                    ZdjecieUrl = "https://www.creativetools.pl/media/catalog/product/cache/560c7478607ae560a8af22f3ea5736c4/S/o/SonyPXWFX6ILMEFX6VKameracyfrowa01_6.jpg" },
                new() { Nazwa = "BMPCC 6K Pro", Producent = "Blackmagic Design", CenaDzien = 200, Kaucja = 2500, IloscDostepna = 3, IloscCalkowita = 3, KategoriaId = 1, Nowosc = true,
                    ZdjecieUrl = "https://u.cyfrowe.pl/600x0/0/4/PocketCinemaCamera6KPRO_1680633903.jpg" },
                new() { Nazwa = "C70 Cinema EOS", Producent = "Canon", CenaDzien = 350, Kaucja = 4000, IloscDostepna = 2, IloscCalkowita = 2, KategoriaId = 1,
                    ZdjecieUrl = "https://cdn11.bigcommerce.com/s-r1a5aln6aa/products/9007/images/17115/CCanon-EOS-C70__64027.1619412669.386.513.jpg?c=1" },
                new() { Nazwa = "FX3 Cinema Line", Producent = "Sony", CenaDzien = 280, Kaucja = 3500, IloscDostepna = 2, IloscCalkowita = 2, KategoriaId = 1,
                    ZdjecieUrl = "https://s.yimg.com/ny/api/res/1.2/TRwJo629ybZhDHmOB0VXgw--/YXBwaWQ9aGlnaGxhbmRlcjt3PTI0MDA7aD0xNTU2/https://s.yimg.com/os/creatr-uploaded-images/2021-02/487e90c0-6f90-11eb-8d75-b81475b35f01" },

                // APARATY (2) - ID: 5-8
                new() { Nazwa = "EOS R5", Producent = "Canon", CenaDzien = 200, Kaucja = 3500, IloscDostepna = 2, IloscCalkowita = 2, KategoriaId = 2, Wyrozniony = true,
                    ZdjecieUrl = "https://foto-net.pl/sklep/obrazki/Canon-EOS-R5-Mark-II-RF-24-105mm-F4L-IS-USM.jpg" },
                new() { Nazwa = "A7S III", Producent = "Sony", CenaDzien = 180, Kaucja = 3000, IloscDostepna = 3, IloscCalkowita = 3, KategoriaId = 2,
                    ZdjecieUrl = "https://image.ceneostatic.pl/data/products/95754755/8a04c63e-c445-4f63-96ff-38cf39ed62df_p-sony-a7s-iii-body-ilce7sm3.jpg" },
                new() { Nazwa = "Z8", Producent = "Nikon", CenaDzien = 220, Kaucja = 4000, IloscDostepna = 2, IloscCalkowita = 2, KategoriaId = 2, Nowosc = true,
                    ZdjecieUrl = "https://www.beiks.com.pl/62085-large_default/nikon-z8-body.jpg" },
                new() { Nazwa = "GH6", Producent = "Panasonic", CenaDzien = 120, Kaucja = 1800, IloscDostepna = 2, IloscCalkowita = 2, KategoriaId = 2,
                    ZdjecieUrl = "https://m.media-amazon.com/images/I/71b4jep8YxL.jpg" },

                // OBIEKTYWY (3) - ID: 9-13
                new() { Nazwa = "24-70mm f/2.8 DG DN Art", Producent = "Sigma", CenaDzien = 80, Kaucja = 1200, IloscDostepna = 3, IloscCalkowita = 3, KategoriaId = 3,
                    ZdjecieUrl = "https://d15k2d11r6t6rl.cloudfront.net/pub/bfra/od1dlfjd/gv0/7fj/2fe/Obiektyw-SIGMA-24-70-mm-F2-8-DG-DN-II-I-Art-469_1_1.jpg" },
                new() { Nazwa = "70-200mm f/2.8 GM OSS II", Producent = "Sony", CenaDzien = 120, Kaucja = 2000, IloscDostepna = 2, IloscCalkowita = 2, KategoriaId = 3, Wyrozniony = true,
                    ZdjecieUrl = "https://www.beiks.com.pl/44357-large_default/sony-fe-70-200mm-f28-gm-oss-ii.jpg" },
                new() { Nazwa = "50mm f/1.2 L USM", Producent = "Canon", CenaDzien = 100, Kaucja = 1800, IloscDostepna = 2, IloscCalkowita = 2, KategoriaId = 3,
                    ZdjecieUrl = "https://www.fotosoft.pl/images/001ARTUR/RF_50_f12_L_USM/rf%2050mm%20f1%201.jpg" },
                new() { Nazwa = "14-24mm f/2.8 DG DN Art", Producent = "Sigma", CenaDzien = 90, Kaucja = 1400, IloscDostepna = 2, IloscCalkowita = 2, KategoriaId = 3,
                    ZdjecieUrl = "https://notopstryk.pl/userdata/public/gfx/32630/PPhoto_14_24_2.8_dgdn_a019_horizontal2.jpg" },
                new() { Nazwa = "85mm f/1.4 DG DN Art", Producent = "Sigma", CenaDzien = 60, Kaucja = 1000, IloscDostepna = 3, IloscCalkowita = 3, KategoriaId = 3,
                    ZdjecieUrl = "https://sigma-foto.pl/hpeciai/896e5ffd4e113d4b5c0cf4bccda99537/pol_pl_Obiektyw-SIGMA-85-mm-F1-4-DG-DN-Art-Sony-E-89_1.jpg" },

                // OŚWIETLENIE (4) - ID: 14-17
                new() { Nazwa = "LS 600d Pro", Producent = "Aputure", CenaDzien = 180, Kaucja = 2000, IloscDostepna = 4, IloscCalkowita = 4, KategoriaId = 4, Wyrozniony = true,
                    ZdjecieUrl = "https://u.cyfrowe.pl/600x0/7/4/aputure_ls_600d_pro_2_826148092.jpg" },
                new() { Nazwa = "300d II", Producent = "Aputure", CenaDzien = 80, Kaucja = 800, IloscDostepna = 6, IloscCalkowita = 6, KategoriaId = 4,
                    ZdjecieUrl = "https://smolinski.tv/wp-content/uploads/2020/06/Aputure-300D-LED-1-600x600.jpg" },
                new() { Nazwa = "MC Pro Panel", Producent = "Aputure", CenaDzien = 35, Kaucja = 300, IloscDostepna = 8, IloscCalkowita = 8, KategoriaId = 4, Nowosc = true,
                    ZdjecieUrl = "https://www.nefal.tv/media/07/f5/7c/1683104506/aputure-mc-pro-rgb-led-panel.jpg?ts=1683812825" },
                new() { Nazwa = "PavoTube II 30C (4-Pack)", Producent = "Nanlite", CenaDzien = 150, Kaucja = 1500, IloscDostepna = 2, IloscCalkowita = 2, KategoriaId = 4,
                    ZdjecieUrl = "https://m.media-amazon.com/images/I/61xsHSbjDvL.jpg" },

                // STATYWY I GIMBLE (5) - ID: 18-21
                new() { Nazwa = "RS 3 Pro", Producent = "DJI", CenaDzien = 100, Kaucja = 1200, IloscDostepna = 3, IloscCalkowita = 3, KategoriaId = 5, Wyrozniony = true,
                    ZdjecieUrl = "https://www.beiks.com.pl/54727-large_default/dji-rs-3-pro.jpg" },
                new() { Nazwa = "aktiv8 Flowtech75", Producent = "Sachtler", CenaDzien = 120, Kaucja = 1500, IloscDostepna = 2, IloscCalkowita = 2, KategoriaId = 5,
                    ZdjecieUrl = "https://www.creativetools.pl/media/catalog/product/cache/560c7478607ae560a8af22f3ea5736c4/S/a/SachtlerSystemaktiv8flowtech75MS01_5.jpg" },
                new() { Nazwa = "Weebill 3", Producent = "Zhiyun", CenaDzien = 60, Kaucja = 600, IloscDostepna = 4, IloscCalkowita = 4, KategoriaId = 5,
                    ZdjecieUrl = "https://m.media-amazon.com/images/I/61WcJNi4NPL.jpg" },
                new() { Nazwa = "Nitrotech 612 + 645 FAST", Producent = "Manfrotto", CenaDzien = 80, Kaucja = 1000, IloscDostepna = 3, IloscCalkowita = 3, KategoriaId = 5,
                    ZdjecieUrl = "https://www.creativetools.pl/media/catalog/product/cache/560c7478607ae560a8af22f3ea5736c4/M/V/MVK612TWINFA_000_5.jpg" },

                // AUDIO (6) - ID: 22-25
                new() { Nazwa = "NTG5", Producent = "Rode", CenaDzien = 50, Kaucja = 500, IloscDostepna = 4, IloscCalkowita = 4, KategoriaId = 6,
                    ZdjecieUrl = "https://audiostacja.pl/wp-content/uploads/2025/10/IMG_1760635300305358987390.png" },
                new() { Nazwa = "F6 Field Recorder", Producent = "Zoom", CenaDzien = 80, Kaucja = 800, IloscDostepna = 2, IloscCalkowita = 2, KategoriaId = 6, Wyrozniony = true,
                    ZdjecieUrl = "https://media.sound-service.eu/Artikelbilder/Shopsystem/890x486/316305_1.jpg" },
                new() { Nazwa = "Wireless GO II", Producent = "Rode", CenaDzien = 45, Kaucja = 400, IloscDostepna = 6, IloscCalkowita = 6, KategoriaId = 6,
                    ZdjecieUrl = "https://www.bemixmedia.pl/wp-content/uploads/2021/03/pol_pl_RODE-Wireless-GO-II-Cyfrowy-dwukanalowy-system-bezprzewodowy-z-wbudowanym-mik-i-rejestratorem-14891_13.jpeg" },
                new() { Nazwa = "MKE 600", Producent = "Sennheiser", CenaDzien = 40, Kaucja = 400, IloscDostepna = 4, IloscCalkowita = 4, KategoriaId = 6,
                    ZdjecieUrl = "https://images.unsplash.com/photo-1590602847861-f357a9332bbc?w=600&h=400&fit=crop" },

                // DRONY (7) - ID: 26-29
                new() { Nazwa = "Mavic 3 Pro Cine", Producent = "DJI", CenaDzien = 250, Kaucja = 3500, IloscDostepna = 2, IloscCalkowita = 2, KategoriaId = 7, Wyrozniony = true, Nowosc = true,
                    ZdjecieUrl = "https://dji-ars.pl/hpeciai/2068c98984dfbc110e9933a049cbc601/pol_pl_Dron-DJI-Mavic-3-Pro-Cine-Premium-Combo-31609_6.jpg" },
                new() { Nazwa = "Inspire 3", Producent = "DJI", CenaDzien = 600, Kaucja = 12000, IloscDostepna = 1, IloscCalkowita = 1, KategoriaId = 7,
                    ZdjecieUrl = "https://u.cyfrowe.pl/600x0/5/1/1_1513229932.jpg" },
                new() { Nazwa = "Mini 4 Pro", Producent = "DJI", CenaDzien = 80, Kaucja = 1000, IloscDostepna = 3, IloscCalkowita = 3, KategoriaId = 7,
                    ZdjecieUrl = "https://ispot.pl/img/imagecache/90001-91000/680x680/1/product-media/90001-91000/DJI-Mini-4-Pro-RC-N2-dron-z-kontrolerem-38227-680x680.webp" },
                new() { Nazwa = "Air 3", Producent = "DJI", CenaDzien = 120, Kaucja = 1500, IloscDostepna = 2, IloscCalkowita = 2, KategoriaId = 7,
                    ZdjecieUrl = "https://dji-ars.pl/hpeciai/aafe04d2bd4e0f33bb991b417c45d915/pol_pl_Dron-DJI-Air-3-RC-N2-33133_3.jpg" },

                // AKCESORIA (8) - ID: 30-35
                new() { Nazwa = "Ninja V+ Monitor/Recorder", Producent = "Atomos", CenaDzien = 80, Kaucja = 800, IloscDostepna = 3, IloscCalkowita = 3, KategoriaId = 8, Wyrozniony = true,
                    ZdjecieUrl = "https://filmowy.org/wp-content/uploads/2022/12/Atomos_Ninja_V_Pro_Kit_1.jpg" },
                new() { Nazwa = "SmallHD Focus Pro 5\"", Producent = "SmallHD", CenaDzien = 100, Kaucja = 1200, IloscDostepna = 2, IloscCalkowita = 2, KategoriaId = 8,
                    ZdjecieUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTQqOBYwl14yEqvV_i8jsLOcgyVz1PoBcNyuw&s" },
                new() { Nazwa = "CFexpress Type B 512GB", Producent = "Sony", CenaDzien = 30, Kaucja = 400, IloscDostepna = 6, IloscCalkowita = 6, KategoriaId = 8,
                    ZdjecieUrl = "https://www.beiks.com.pl/24159-large_default/karta-pamiec-sony-512gb-cfexpress-typu-b-tough-r1700w1480-.jpg" },
                new() { Nazwa = "V-Mount Battery Kit", Producent = "Core SWX", CenaDzien = 50, Kaucja = 600, IloscDostepna = 4, IloscCalkowita = 4, KategoriaId = 8,
                    ZdjecieUrl = "https://www.bemixmedia.pl/wp-content/uploads/2022/07/Core-SWX-PBE-A7FZ.jpeg" },
                new() { Nazwa = "Camera Cage for A7S III", Producent = "SmallRig", CenaDzien = 15, Kaucja = 150, IloscDostepna = 4, IloscCalkowita = 4, KategoriaId = 8,
                    ZdjecieUrl = "https://www.creativetools.pl/media/catalog/product/cache/560c7478607ae560a8af22f3ea5736c4/S/m/SmallRig2999CameraCageforSonyAlpha7SIIIA7SIIIA7S301_5.jpg" },
                new() { Nazwa = "Follow Focus Wireless", Producent = "Tilta", CenaDzien = 70, Kaucja = 800, IloscDostepna = 2, IloscCalkowita = 2, KategoriaId = 8,
                    ZdjecieUrl = "https://www.creativetools.pl/media/catalog/product/cache/560c7478607ae560a8af22f3ea5736c4/T/i/TiltaWLCT03NucleusMWirelessFollowFocusSystem_4.jpg" }
            });
            await context.SaveChangesAsync();
        }

        // Przykładowe wypożyczenia
        if (!context.Wypozyczenia.Any())
        {
            jan = await userManager.FindByEmailAsync("klient@test.pl");
            anna = await userManager.FindByEmailAsync("anna@filmstudio.pl");
            piotr = await userManager.FindByEmailAsync("piotr@weddings.pl");
            marta = await userManager.FindByEmailAsync("marta@docs.pl");
            tomek = await userManager.FindByEmailAsync("tomek@agencja.pl");

            if (jan != null && anna != null && piotr != null && marta != null && tomek != null)
            {
                var wypozyczenia = new List<(Wypozyczenie wyp, List<(int sprzetId, decimal cena, int ilosc)> pozycje)>
                {
                    // ============ STYCZEŃ 2026 - ZAKOŃCZONE ============
                    
                    // 1. Sony FX6 - produkcja reklamowa (Jan)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 1, 5), DataDo = new DateTime(2026, 1, 8),
                        Status = StatusWypozyczenia.Zakonczone, KosztCalkowity = 1800, Kaucja = 5000,
                        UzytkownikId = jan.Id, DataUtworzenia = new DateTime(2026, 1, 3),
                        UwagiKlienta = "Produkcja reklamowa dla klienta z branży motoryzacyjnej"
                    }, new List<(int, decimal, int)> { (1, 450, 1) }),

                    // 2. Canon R5 + obiektyw - sesja produktowa (Anna)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 1, 10), DataDo = new DateTime(2026, 1, 15),
                        Status = StatusWypozyczenia.Zakonczone, KosztCalkowity = 1680, Kaucja = 4700,
                        UzytkownikId = anna.Id, DataUtworzenia = new DateTime(2026, 1, 8),
                        UwagiKlienta = "Sesja produktowa dla sklepu internetowego"
                    }, new List<(int, decimal, int)> { (5, 200, 1), (9, 80, 1) }),

                    // 3. Sony A7S III + gimbal - wesele (Piotr)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 1, 18), DataDo = new DateTime(2026, 1, 19),
                        Status = StatusWypozyczenia.Zakonczone, KosztCalkowity = 460, Kaucja = 3600,
                        UzytkownikId = piotr.Id, DataUtworzenia = new DateTime(2026, 1, 15),
                        UwagiKlienta = "Wesele w Konstancinie"
                    }, new List<(int, decimal, int)> { (6, 180, 1), (18, 100, 1) }),

                    // 4. Duży zestaw oświetleniowy (Marta)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 1, 20), DataDo = new DateTime(2026, 1, 25),
                        Status = StatusWypozyczenia.Zakonczone, KosztCalkowity = 1980, Kaucja = 4300,
                        UzytkownikId = marta.Id, DataUtworzenia = new DateTime(2026, 1, 18),
                        UwagiKlienta = "Film dokumentalny o artyście"
                    }, new List<(int, decimal, int)> { (14, 180, 1), (15, 80, 2), (17, 150, 1) }),

                    // 5. Dron Mavic 3 Pro (Tomek)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 1, 22), DataDo = new DateTime(2026, 1, 24),
                        Status = StatusWypozyczenia.Zakonczone, KosztCalkowity = 750, Kaucja = 3500,
                        UzytkownikId = tomek.Id, DataUtworzenia = new DateTime(2026, 1, 20),
                        UwagiKlienta = "Zdjęcia z drona dla dewelopera"
                    }, new List<(int, decimal, int)> { (26, 250, 1) }),

                    // ============ LUTY 2026 - W TRAKCIE / POTWIERDZONE / OCZEKUJĄCE ============

                    // 6. BMPCC 6K Pro + audio - W TRAKCIE (Anna)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 1, 30), DataDo = new DateTime(2026, 2, 5),
                        Status = StatusWypozyczenia.WTrakcie, KosztCalkowity = 1715, Kaucja = 3700,
                        UzytkownikId = anna.Id, DataUtworzenia = new DateTime(2026, 1, 28),
                        UwagiKlienta = "Film dokumentalny - kontynuacja projektu"
                    }, new List<(int, decimal, int)> { (2, 200, 1), (23, 80, 1), (24, 45, 1) }),

                    // 7. RS 3 Pro + monitor - W TRAKCIE (Piotr)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 2, 1), DataDo = new DateTime(2026, 2, 3),
                        Status = StatusWypozyczenia.WTrakcie, KosztCalkowity = 540, Kaucja = 2000,
                        UzytkownikId = piotr.Id, DataUtworzenia = new DateTime(2026, 1, 29),
                        UwagiKlienta = "Nagranie eventu firmowego"
                    }, new List<(int, decimal, int)> { (18, 100, 1), (30, 80, 1) }),

                    // 8. Canon C70 + obiektywy - W TRAKCIE (Tomek)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 2, 1), DataDo = new DateTime(2026, 2, 7),
                        Status = StatusWypozyczenia.WTrakcie, KosztCalkowity = 3710, Kaucja = 7200,
                        UzytkownikId = tomek.Id, DataUtworzenia = new DateTime(2026, 1, 28),
                        UwagiKlienta = "Spot reklamowy dla sieci restauracji"
                    }, new List<(int, decimal, int)> { (3, 350, 1), (10, 120, 1), (11, 100, 1) }),

                    // 9. Nikon Z8 + statywy - POTWIERDZONE (Jan)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 2, 8), DataDo = new DateTime(2026, 2, 12),
                        Status = StatusWypozyczenia.Potwierdzone, KosztCalkowity = 1700, Kaucja = 5500,
                        UzytkownikId = jan.Id, DataUtworzenia = new DateTime(2026, 2, 1),
                        UwagiKlienta = "Sesja wizerunkowa dla firmy IT"
                    }, new List<(int, decimal, int)> { (7, 220, 1), (19, 120, 1) }),

                    // 10. Sony FX3 + Wireless GO - POTWIERDZONE (Marta)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 2, 10), DataDo = new DateTime(2026, 2, 14),
                        Status = StatusWypozyczenia.Potwierdzone, KosztCalkowity = 1625, Kaucja = 3900,
                        UzytkownikId = marta.Id, DataUtworzenia = new DateTime(2026, 2, 1),
                        UwagiKlienta = "Wywiady do dokumentu"
                    }, new List<(int, decimal, int)> { (4, 280, 1), (24, 45, 1) }),

                    // 11. Sony A7S III - walentynki - POTWIERDZONE (Piotr)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 2, 14), DataDo = new DateTime(2026, 2, 16),
                        Status = StatusWypozyczenia.Potwierdzone, KosztCalkowity = 780, Kaucja = 4000,
                        UzytkownikId = piotr.Id, DataUtworzenia = new DateTime(2026, 2, 1),
                        UwagiKlienta = "Walentynkowa sesja narzeczeńska"
                    }, new List<(int, decimal, int)> { (6, 180, 1), (13, 60, 1), (16, 35, 2) }),

                    // 12. Panasonic GH6 + PavoTubes - POTWIERDZONE (Anna)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 2, 15), DataDo = new DateTime(2026, 2, 18),
                        Status = StatusWypozyczenia.Potwierdzone, KosztCalkowity = 1080, Kaucja = 3300,
                        UzytkownikId = anna.Id, DataUtworzenia = new DateTime(2026, 2, 2),
                        UwagiKlienta = "Teledysk dla lokalnego zespołu"
                    }, new List<(int, decimal, int)> { (8, 120, 1), (17, 150, 1) }),

                    // 13. Aputure 600d + 300d - OCZEKUJĄCE (Tomek)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 2, 18), DataDo = new DateTime(2026, 2, 22),
                        Status = StatusWypozyczenia.Oczekujace, KosztCalkowity = 1300, Kaucja = 2800,
                        UzytkownikId = tomek.Id, DataUtworzenia = new DateTime(2026, 2, 1),
                        UwagiKlienta = "Sesja katalogowa - duży klient"
                    }, new List<(int, decimal, int)> { (14, 180, 1), (15, 80, 1) }),

                    // 14. Zestaw audio - OCZEKUJĄCE (Jan)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 2, 20), DataDo = new DateTime(2026, 2, 25),
                        Status = StatusWypozyczenia.Oczekujace, KosztCalkowity = 1020, Kaucja = 1700,
                        UzytkownikId = jan.Id, DataUtworzenia = new DateTime(2026, 2, 1),
                        UwagiKlienta = "Nagranie podcastu video"
                    }, new List<(int, decimal, int)> { (22, 50, 1), (23, 80, 1), (24, 45, 2) }),

                    // 15. DJI Mini 4 Pro - OCZEKUJĄCE (Piotr)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 2, 22), DataDo = new DateTime(2026, 2, 23),
                        Status = StatusWypozyczenia.Oczekujace, KosztCalkowity = 160, Kaucja = 1000,
                        UzytkownikId = piotr.Id, DataUtworzenia = new DateTime(2026, 2, 1),
                        UwagiKlienta = "Zdjęcia z drona na weselu"
                    }, new List<(int, decimal, int)> { (28, 80, 1) }),

                    // 16. Sony FX6 + pełen zestaw - OCZEKUJĄCE (Marta)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 2, 24), DataDo = new DateTime(2026, 2, 28),
                        Status = StatusWypozyczenia.Oczekujace, KosztCalkowity = 3225, Kaucja = 8200,
                        UzytkownikId = marta.Id, DataUtworzenia = new DateTime(2026, 2, 1),
                        UwagiKlienta = "Główne zdjęcia do dokumentu - 5 dni"
                    }, new List<(int, decimal, int)> { (1, 450, 1), (10, 120, 1), (18, 100, 1), (22, 50, 1) }),

                    // 17. DJI Inspire 3 - OCZEKUJĄCE (Anna)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 2, 25), DataDo = new DateTime(2026, 2, 27),
                        Status = StatusWypozyczenia.Oczekujace, KosztCalkowity = 1800, Kaucja = 12000,
                        UzytkownikId = anna.Id, DataUtworzenia = new DateTime(2026, 2, 1),
                        UwagiKlienta = "Duża produkcja - zdjęcia lotnicze nad Warszawą"
                    }, new List<(int, decimal, int)> { (27, 600, 1) }),

                    // ============ MARZEC 2026 - POTWIERDZONE ============

                    // 18. Canon C70 + oświetlenie - POTWIERDZONE (Tomek)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 3, 1), DataDo = new DateTime(2026, 3, 5),
                        Status = StatusWypozyczenia.Potwierdzone, KosztCalkowity = 2550, Kaucja = 6500,
                        UzytkownikId = tomek.Id, DataUtworzenia = new DateTime(2026, 1, 25),
                        UwagiKlienta = "Film korporacyjny dla banku"
                    }, new List<(int, decimal, int)> { (3, 350, 1), (14, 180, 1), (21, 80, 1) }),

                    // 19. BMPCC 6K Pro + akcesoria - POTWIERDZONE (Jan)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 3, 2), DataDo = new DateTime(2026, 3, 6),
                        Status = StatusWypozyczenia.Potwierdzone, KosztCalkowity = 1475, Kaucja = 4050,
                        UzytkownikId = jan.Id, DataUtworzenia = new DateTime(2026, 1, 28),
                        UwagiKlienta = "Krótki metraż - projekt własny"
                    }, new List<(int, decimal, int)> { (2, 200, 1), (30, 80, 1), (34, 15, 1) }),

                    // 20. Sony A7S III + 85mm - sesja portretowa - POTWIERDZONE (Piotr)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 3, 8), DataDo = new DateTime(2026, 3, 10),
                        Status = StatusWypozyczenia.Potwierdzone, KosztCalkowity = 720, Kaucja = 4000,
                        UzytkownikId = piotr.Id, DataUtworzenia = new DateTime(2026, 1, 25),
                        UwagiKlienta = "Sesja ślubna w studio"
                    }, new List<(int, decimal, int)> { (6, 180, 1), (13, 60, 1) }),

                    // 21. Zestaw gimbalowy - POTWIERDZONE (Marta)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 3, 10), DataDo = new DateTime(2026, 3, 15),
                        Status = StatusWypozyczenia.Potwierdzone, KosztCalkowity = 960, Kaucja = 1800,
                        UzytkownikId = marta.Id, DataUtworzenia = new DateTime(2026, 2, 1),
                        UwagiKlienta = "Dynamiczne ujęcia do dokumentu"
                    }, new List<(int, decimal, int)> { (18, 100, 1), (20, 60, 1) }),

                    // 22. Canon R5 + 70-200mm - POTWIERDZONE (Anna)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 3, 12), DataDo = new DateTime(2026, 3, 14),
                        Status = StatusWypozyczenia.Potwierdzone, KosztCalkowity = 960, Kaucja = 5500,
                        UzytkownikId = anna.Id, DataUtworzenia = new DateTime(2026, 2, 1),
                        UwagiKlienta = "Reportaż eventowy"
                    }, new List<(int, decimal, int)> { (5, 200, 1), (10, 120, 1) }),

                    // 23. Mavic 3 Pro + Air 3 - POTWIERDZONE (Tomek)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 3, 15), DataDo = new DateTime(2026, 3, 18),
                        Status = StatusWypozyczenia.Potwierdzone, KosztCalkowity = 1480, Kaucja = 5000,
                        UzytkownikId = tomek.Id, DataUtworzenia = new DateTime(2026, 2, 1),
                        UwagiKlienta = "Zdjęcia nieruchomości premium"
                    }, new List<(int, decimal, int)> { (26, 250, 1), (29, 120, 1) }),

                    // 24. Nikon Z8 + 14-24mm - krajobraz - POTWIERDZONE (Jan)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 3, 20), DataDo = new DateTime(2026, 3, 25),
                        Status = StatusWypozyczenia.Potwierdzone, KosztCalkowity = 1860, Kaucja = 5400,
                        UzytkownikId = jan.Id, DataUtworzenia = new DateTime(2026, 2, 1),
                        UwagiKlienta = "Projekt fotografii krajobrazowej - Bieszczady"
                    }, new List<(int, decimal, int)> { (7, 220, 1), (12, 90, 1) }),

                    // 25. Pełen zestaw produkcyjny - POTWIERDZONE (Anna)
                    (new Wypozyczenie {
                        DataOd = new DateTime(2026, 3, 22), DataDo = new DateTime(2026, 3, 28),
                        Status = StatusWypozyczenia.Potwierdzone, KosztCalkowity = 5390, Kaucja = 12700,
                        UzytkownikId = anna.Id, DataUtworzenia = new DateTime(2026, 2, 1),
                        UwagiKlienta = "Duża produkcja - film reklamowy ogólnopolski"
                    }, new List<(int, decimal, int)> { (1, 450, 1), (10, 120, 1), (14, 180, 1), (18, 100, 1), (23, 80, 1) })
                };

                foreach (var (wyp, pozycje) in wypozyczenia)
                {
                    context.Wypozyczenia.Add(wyp);
                    await context.SaveChangesAsync();

                    foreach (var (sprzetId, cena, ilosc) in pozycje)
                    {
                        var dni = (wyp.DataDo - wyp.DataOd).Days + 1;
                        context.PozycjeWypozyczen.Add(new PozycjaWypozyczenia
                        {
                            WypozyczenieId = wyp.Id,
                            SprzetId = sprzetId,
                            Ilosc = ilosc,
                            CenaDzien = cena,
                            KosztPozycji = cena * ilosc * dni
                        });
                    }
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}