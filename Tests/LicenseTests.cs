using Xunit;

public class LicenseTests
{
    [Fact]
    public void SuspendedLicense_DeniesDownload_WithoutChangingCount()
    {
        // Paruošiame licenciją su vienu panaudotu atsisiuntimu.
        License licencija = new License(1, 15, 3, true);
        Assert.True(licencija.TryUseDownload());

        licencija.Suspend();

        // Atliekame tikrinamą veiksmą.
        bool allowed = licencija.TryUseDownload();

        // Patikriname rezultatą.
        Assert.False(allowed);
        Assert.Equal(1, licencija.DownloadCount);
    }

    [Fact]
    public void ReachedLimit_DeniesDownload_WithoutChangingCount()
    {
        License licencija = new License(1, 15, 3, true);

        for(int i = 0; i < 3; i++) // 3 kartus bandoma atsisiųsti ir dar vėliau 1 kartą, nors limit yra 3
        {
            Assert.True(licencija.TryUseDownload());
            
        }
        bool allowed = licencija.TryUseDownload(); // ketvirtas bandymas atsisiųsti (turi būti atmestas)

        Assert.False(allowed);
        Assert.Equal(3, licencija.DownloadCount);
    }
}