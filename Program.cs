using System;

class Program
{
    static void Main()
    {
        Product produktas = new Product(
            1, "Shake Presets", 47m);

        License licenzija = new License(
            produktas.Id, 15, 3, true);

        Console.WriteLine(
            $"Produktas: {produktas.Name}, kaina: {produktas.Price}");

        while (true)
        {
            Console.WriteLine(
                $"\nAr norėtumėte atsisiųsti {produktas.Name}?");
            Console.WriteLine(
                "Įveskite true, o norėdami baigti – false.");

            string? input = Console.ReadLine();

            if (input is null)
            {
                break;
            }

            if (!bool.TryParse(input, out bool wantsDownload))
            {
                Console.WriteLine("Netinkama įvestis. Rašykite true arba false.");
                continue;
            }

            if (!wantsDownload)
            {
                break;
            }

            bool downloadAllowed = licenzija.TryUseDownload();

            int remainingDownloads =
                licenzija.DownloadLimit - licenzija.DownloadCount;

            remainingDownloads = Math.Max(0, remainingDownloads);

            if (downloadAllowed)
            {
                Console.WriteLine("Atsisiuntimo bandymas leistas.");
            }
            else
            {
                Console.WriteLine(
                    "Atsisiuntimas neleidžiamas: licencija sustabdyta arba pasiektas limitas.");
            }

            Console.WriteLine(
                $"Liko atsisiuntimų: {remainingDownloads}");
        }
    }
}