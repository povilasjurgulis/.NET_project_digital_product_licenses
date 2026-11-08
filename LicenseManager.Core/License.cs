using System;

public class License
{
    // Fields
    public int ProductId { get; private set; }
    public int LicenseId { get; private set; }
    public string LicenseKey {get; private set; }
    public int DownloadLimit { get; private set; }
    public int DownloadCount { get; private set; }
    public bool IsActive { get; private set; }

    public int OrderItemId { get; private set; } // Rodo, už kurią užsakymo eilutę licencija išduota

    public OrderItem OrderItem { get; private set; } = null!;

    // Konstruktorius:
    public License(int newProductId, int newLicenseId, int newDownloadLimit, bool newIsActive)
    {
        if (newProductId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newProductId), "Produkto ID turi būti teigiamas.");
        }

        if (newLicenseId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newLicenseId), "Licencijos ID turi būti teigiamas.");
        }

        if (newDownloadLimit < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newDownloadLimit), "Limitas negali būti neigiamas.");
        }

        ProductId = newProductId;
        LicenseId = newLicenseId;
        LicenseKey = Guid.NewGuid().ToString("N"); // sugeneruoja UUID, o ToString("N") pateikia jį kaip 32 šešioliktainius simbolius be brūkšnelių
        DownloadLimit = newDownloadLimit;
        DownloadCount = 0;
        IsActive = newIsActive;
    }

    // Šis konstruktorius naudojamas naujai licencijai sukurti ir automatiškai nustatyti visas jos pradines reikšmes:
    public License(OrderItem newOrderItem, int newDownloadLimit)
    {
        if (newOrderItem is null)
        {
            throw new ArgumentNullException(nameof(newOrderItem));
        }

        if (newDownloadLimit < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(newDownloadLimit),
                "Atsisiuntimų limitas negali būti neigiamas.");
        }

        OrderItem = newOrderItem;
        OrderItemId = newOrderItem.OrderItemId;

        ProductId = newOrderItem.ProductId;

        DownloadLimit = newDownloadLimit;
        DownloadCount = 0;
        IsActive = true;
        
        LicenseKey = Guid.NewGuid().ToString("N"); // Sugeneruojame unikalų license key

        // License ID suteiks duomenų bazė
    }

    // Šis konstruktorius reikalingas EF Core, kad jis galėtų sukurti License objektą skaitydamas licenciją iš SQLite db
    private License()
    {
        LicenseKey = string.Empty;
    }


    // Metodai:
    public bool CanDownload()
    {
        return IsActive && DownloadCount < DownloadLimit;
    }

    public bool TryUseDownload()
    {
        if (!CanDownload())
        {
            return false;
        }

        DownloadCount++;
        return true;
    }

    public void ResetDownloads()
    {
        DownloadCount = 0;
    }

    public void Suspend()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void ChangeDownloadLimit(int newLimit)
    {
        if (newLimit < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newLimit), "Limitas negali būti neigiamas.");
        }

        DownloadLimit = newLimit;
    }
}