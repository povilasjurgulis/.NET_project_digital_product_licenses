using System;

class Program
{
    static void Main()
    {
        Product produktas = new Product(1, "Shake Presets", 47m);
        License licencija = new License(produktas.Id, 15, 3, true);

        Console.WriteLine($"Produktas: {produktas.Name}, kaina: {produktas.Price}");

        // Vienas ciklas leidžia pasirinkti veiksmus bet kuria tvarka.
        while (true)
        {
            int remainingDownloads = Math.Max(0, licencija.DownloadLimit - licencija.DownloadCount);

            Console.WriteLine($"\nLicencija aktyvi: {licencija.IsActive}");
            Console.WriteLine($"Panaudota atsisiuntimų: {licencija.DownloadCount} iš {licencija.DownloadLimit}. " + $"Liko: {remainingDownloads}.");

            Console.WriteLine("1 - Bandyti atsisiųsti");
            Console.WriteLine("2 - Sustabdyti licenciją");
            Console.WriteLine("3 - Aktyvuoti licenciją");
            Console.WriteLine("4 - Atstatyti atsisiuntimų skaičių");
            Console.WriteLine("0 - Baigti");
            Console.Write("Pasirinkite veiksmą: ");

            string? input = Console.ReadLine(); // ? rodo, kad gali būti ir null reikšmė

            if (input is null)
            {
                return;
            }

            input = input.Trim(); // pašalina tarpus įvesties pradžioje ir pabaigoje, todėl, pavyzdžiui, " 1 " tampa "1"

            if (input == "0")
            {
                return;
            }
            else if (input == "1") // jeigu naudotojas nori atsisiųsti failą
            {
                // Šis metodas kviečiamas vieną kartą vienam bandymui.
                bool downloadAllowed = licencija.TryUseDownload();

                if (downloadAllowed)
                {
                    Console.WriteLine("Atsisiuntimo bandymas leistas.");
                }
                else if (!licencija.IsActive)
                {
                    Console.WriteLine("Atsisiuntimas neleidžiamas: licencija sustabdyta.");
                }
                else
                {
                    Console.WriteLine("Atsisiuntimas neleidžiamas: pasiektas limitas.");
                }
            }
            else if (input == "2") // jeigu nori sustabdyti licenciją
            {
                if (licencija.IsActive)
                {
                    licencija.Suspend();
                    Console.WriteLine("Licencija sustabdyta.");
                }
                else
                {
                    Console.WriteLine("Licencija jau sustabdyta.");
                }
            }
            else if (input == "3") // jeigu nori aktyvuoti licenciją
            {
                if (!licencija.IsActive)
                {
                    licencija.Activate();
                    Console.WriteLine("Licencija aktyvuota. Atsisiuntimų skaitiklis nepakeistas.");
                }
                else
                {
                    Console.WriteLine("Licencija jau aktyvi.");
                }
            }
            else if (input == "4") // jeigu nori atstatyti atsisiuntimų skaičių
            {
                licencija.ResetDownloads();
                Console.WriteLine("Atsisiuntimų skaitiklis atstatytas. Licencijos aktyvumas nepakeistas.");
            }
            else
            {
                Console.WriteLine("Netinkamas pasirinkimas. Įveskite 0, 1, 2, 3 arba 4.");
            }
        }
    }
}
