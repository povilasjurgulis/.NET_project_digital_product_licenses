using System;

class Program
{
    static void Main()
    {
        Product produktas = new Product(1, "Shake Presets", 47m);
        License licenzija = new License(produktas.Id, 15, 3, true);

        Console.WriteLine($"Produktas: {produktas.Name}, kaina: {produktas.Price}");

        // Vienas ciklas leidžia pasirinkti veiksmus bet kuria tvarka.
        while (true)
        {
            int remainingDownloads = Math.Max(0, licenzija.DownloadLimit - licenzija.DownloadCount);

            Console.WriteLine($"\nLicencija aktyvi: {licenzija.IsActive}");
            Console.WriteLine($"Panaudota atsisiuntimų: {licenzija.DownloadCount} iš {licenzija.DownloadLimit}. " + $"Liko: {remainingDownloads}.");

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

            input = input.Trim();

            if (input == "0")
            {
                return;
            }
            else if (input == "1") // jeigu naudotojas nori atsisiųsti failą
            {
                // Šis metodas kviečiamas vieną kartą vienam bandymui.
                bool downloadAllowed = licenzija.TryUseDownload();

                if (downloadAllowed)
                {
                    Console.WriteLine("Atsisiuntimo bandymas leistas.");
                }
                else if (!licenzija.IsActive)
                {
                    Console.WriteLine("Atsisiuntimas neleidžiamas: licencija sustabdyta.");
                }
                else
                {
                    Console.WriteLine("Atsisiuntimas neleidžiamas: pasiektas limitas.");
                }
            }
            else if (input == "2") // jeigu nori sustabdyti licenziją
            {
                if (licenzija.IsActive)
                {
                    licenzija.Suspend();
                    Console.WriteLine("Licencija sustabdyta.");
                }
                else
                {
                    Console.WriteLine("Licencija jau sustabdyta.");
                }
            }
            else if (input == "3") // jeigu nori aktyvuoti licenziją
            {
                if (!licenzija.IsActive)
                {
                    licenzija.Activate();
                    Console.WriteLine("Licencija aktyvuota. Atsisiuntimų skaitiklis nepakeistas.");
                }
                else
                {
                    Console.WriteLine("Licencija jau aktyvi.");
                }
            }
            else if (input == "4") // jeigu nori atstatyti atsisiuntimų skaičių
            {
                licenzija.ResetDownloads();
                Console.WriteLine("Skaitiklis atstatytas. Licencijos aktyvumas nepakeistas.");
            }
            else
            {
                Console.WriteLine("Netinkamas pasirinkimas. Įveskite 0, 1, 2, 3 arba 4.");
            }
        }
    }
}
