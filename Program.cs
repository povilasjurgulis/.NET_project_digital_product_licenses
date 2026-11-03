using System;
using System.Diagnostics.Contracts;

class Program
{
    static void Main()
    {
        Product produktas = new Product(1, "Shake Presets", 47m);
        License licenzija = new License(produktas.Id, 15, 3, true);

        Console.WriteLine($"Produktas: {produktas.Name}, kaina: {produktas.Price}");

        while (true)
        {
            Console.WriteLine($"\nAr norėtumėte atsisiųsti {produktas.Name}?");
            Console.WriteLine("Įveskite true, o norėdami baigti – false.");
            string? input = Console.ReadLine(); // gali būti ir null

            if (input is null)
            {
                break;
            }
            if (!bool.TryParse(input, out bool wantsDownload)) // tikriname, ar tekstą pavyko konvertuoti į bool (true arba false)
            {
                Console.WriteLine("Netinkama įvestis. Rašykite true arba false.");
                continue;
            }
            if (!wantsDownload) // jeigu įveda false
            {
                break;
            }

            bool downloadAllowed = licenzija.TryUseDownload();
            int remainingDownloads = licenzija.DownloadLimit - licenzija.DownloadCount;
            remainingDownloads = Math.Max(0, remainingDownloads);

            if (downloadAllowed)
            {
                Console.WriteLine("Atsisiuntimo bandymas leistas.");
            }
            else
            {
                Console.WriteLine("Atsisiuntimas neleidžiamas: licencija sustabdyta arba pasiektas limitas.");
            }

            Console.WriteLine($"Liko atsisiuntimų: {remainingDownloads}");

            if(remainingDownloads == 0)
            {
                Console.WriteLine("\nAr norėtumėte atstatyti (reset) atsisiuntimų kiekį?\nĮveskite true ar false:");
                string? inputReset = Console.ReadLine();

                if(inputReset is null)
                {
                    break;
                }
                if(!(bool.TryParse(inputReset, out bool wantsReset)))
                {
                    Console.WriteLine("Netinkama įvestis. Rašykite true arba false");
                    continue;
                }
                if (!wantsReset)
                {
                    break;
                }

                licenzija.ResetDownloads();
                Console.WriteLine($"Jūsų atsisiuntimų kiekis atstatytas. Jums liko {licenzija.DownloadLimit - licenzija.DownloadCount} atsisiuntimai");
            }
        }
    }
}