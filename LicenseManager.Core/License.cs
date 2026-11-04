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