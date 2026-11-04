using System;
using Xunit;

public class LicenseTests
{
    // Konstruktorius turi teisingai išsaugoti pateiktus duomenis.
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Constructor_Correctly_SavesData(bool newIsActive)
    {
        License licencija = new License(75, 180, 5, newIsActive);

        Assert.Equal(75, licencija.ProductId);
        Assert.Equal(180, licencija.LicenseId);
        Assert.Equal(5, licencija.DownloadLimit);
        Assert.Equal(newIsActive, licencija.IsActive);
    }

    // Produkto ID turi būti teigiamas.
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_InvalidProductId_ThrowsException(int productId)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new License(productId, 15, 3, true));
    }

    // Licencijos ID turi būti teigiamas.
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_InvalidLicenseId_ThrowsException(int licenseId)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new License(1, licenseId, 3, true));
    }

    // Neigiamas limitas neleidžiamas.
    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Constructor_NegativeLimit_ThrowsException(int limit)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new License(1, 15, limit, true));
    }

    // CanDownload() turi tik patikrinti leidimą.
    // Pakartotinis jo kvietimas negali panaudoti atsisiuntimų.
    [Fact]
    public void CanDownload_DoesNotChangeDownloadCount()
    {
        License licencija = new License(1, 15, 3, true);
        Assert.True(licencija.TryUseDownload());

        Assert.True(licencija.CanDownload());
        Assert.True(licencija.CanDownload());

        Assert.Equal(1, licencija.DownloadCount);
    }

    // Aktyvi licencija su likusiu limitu leidžia atsisiųsti.
    [Fact]
    public void ActiveLicense_AllowsDownload_AndIncreasesCount()
    {
        License licencija = new License(1, 15, 3, true);

        bool allowed = licencija.TryUseDownload();

        Assert.True(allowed);
        Assert.Equal(1, licencija.DownloadCount);
    }

    // Iš karto neaktyvia sukurta licencija atsisiųsti neleidžia.
    [Fact]
    public void InitiallyInactiveLicense_DeniesDownload()
    {
        License licencija = new License(1, 15, 3, false);

        Assert.False(licencija.CanDownload());

        bool allowed = licencija.TryUseDownload();

        Assert.False(allowed);
        Assert.Equal(0, licencija.DownloadCount);
    }

    // Nulinis limitas galimas, tačiau atsisiųsti neleidžiama.
    [Fact]
    public void ZeroLimit_DeniesDownload_WithoutChangingCount()
    {
        License licencija = new License(1, 15, 0, true);

        Assert.False(licencija.CanDownload());

        bool allowed = licencija.TryUseDownload();

        Assert.False(allowed);
        Assert.Equal(0, licencija.DownloadCount);
    }

    // Pirmi trys bandymai leidžiami, vėlesni atmetami.
    [Fact]
    public void ReachedLimit_DeniesFurtherDownloads_WithoutChangingCount()
    {
        License licencija = new License(1, 15, 3, true);

        for (int i = 0; i < 3; i++)
        {
            Assert.True(licencija.TryUseDownload());
        }

        Assert.False(licencija.CanDownload());
        Assert.False(licencija.TryUseDownload());
        Assert.False(licencija.TryUseDownload());

        Assert.Equal(3, licencija.DownloadCount);
    }

    // Sustabdymas turi uždrausti atsisiuntimą,
    // tačiau išsaugoti skaitiklį ir limitą.
    [Fact]
    public void Suspend_DeniesDownload_WithoutChangingCountOrLimit()
    {
        License licencija = new License(1, 15, 3, true);
        Assert.True(licencija.TryUseDownload());

        licencija.Suspend();

        Assert.False(licencija.IsActive);
        Assert.False(licencija.CanDownload());
        Assert.False(licencija.TryUseDownload());

        Assert.Equal(1, licencija.DownloadCount);
        Assert.Equal(3, licencija.DownloadLimit);
    }

    // Aktyvavimas turi atkurti prieigą,
    // jeigu likęs atsisiuntimų limitas.
    [Fact]
    public void Activate_RestoresAccess_WithoutResettingCount()
    {
        License licencija = new License(1, 15, 3, true);
        Assert.True(licencija.TryUseDownload());
        licencija.Suspend();

        licencija.Activate();

        Assert.True(licencija.IsActive);
        Assert.True(licencija.CanDownload());
        Assert.Equal(1, licencija.DownloadCount);
        Assert.Equal(3, licencija.DownloadLimit);
    }

    // Aktyvavimas negali panaikinti išnaudoto limito.
    [Fact]
    public void Activate_WhenLimitReached_DoesNotAllowDownload()
    {
        License licencija = new License(1, 15, 3, true);

        for (int i = 0; i < 3; i++)
        {
            Assert.True(licencija.TryUseDownload());
        }

        licencija.Suspend();
        licencija.Activate();

        Assert.True(licencija.IsActive);
        Assert.False(licencija.CanDownload());
        Assert.False(licencija.TryUseDownload());

        Assert.Equal(3, licencija.DownloadCount);
        Assert.Equal(3, licencija.DownloadLimit);
    }

    // Atstatymas turi veikti tiek aktyviai, tiek sustabdytai licencijai.
    // Jis negali pakeisti aktyvumo ar limito.
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ResetDownloads_ClearsCount_PreservesStatusAndLimit(
        bool isActive)
    {
        License licencija = new License(1, 15, 3, true);

        Assert.True(licencija.TryUseDownload());
        Assert.True(licencija.TryUseDownload());

        if (!isActive)
        {
            licencija.Suspend();
        }

        licencija.ResetDownloads();

        Assert.Equal(0, licencija.DownloadCount);
        Assert.Equal(3, licencija.DownloadLimit);
        Assert.Equal(isActive, licencija.IsActive);

        // Po atstatymo atsisiųsti galima tik aktyvia licencija.
        Assert.Equal(isActive, licencija.CanDownload());
    }

    // Išnaudotos aktyvios licencijos skaitiklio atstatymas
    // turi leisti vėl naudoti visą limitą.
    [Fact]
    public void ResetDownloads_AfterLimitReached_AllowsDownloadsAgain()
    {
        License licencija = new License(1, 15, 3, true);

        for (int i = 0; i < 3; i++)
        {
            Assert.True(licencija.TryUseDownload());
        }

        Assert.False(licencija.TryUseDownload());

        licencija.ResetDownloads();

        for (int i = 0; i < 3; i++)
        {
            Assert.True(licencija.TryUseDownload());
        }

        Assert.False(licencija.TryUseDownload());
        Assert.Equal(3, licencija.DownloadCount);
    }

    // Panaudota 2. Tikriname naują limitą žemiau,
    // ties ir aukščiau panaudoto skaičiaus.
    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, false)]
    [InlineData(3, true)]
    [InlineData(5, true)]
    public void ChangeDownloadLimit_UpdatesLimit_PreservesCount(
        int newLimit,
        bool expectedCanDownload)
    {
        License licencija = new License(1, 15, 3, true);

        Assert.True(licencija.TryUseDownload());
        Assert.True(licencija.TryUseDownload());

        licencija.ChangeDownloadLimit(newLimit);

        Assert.Equal(newLimit, licencija.DownloadLimit);
        Assert.Equal(2, licencija.DownloadCount);
        Assert.True(licencija.IsActive);
        Assert.Equal(expectedCanDownload, licencija.CanDownload());
    }

    // Padidinus išnaudotą limitą turi atsirasti papildomų atsisiuntimų.
    [Fact]
    public void IncreaseExhaustedLimit_AllowsAdditionalDownload()
    {
        License licencija = new License(1, 15, 3, true);

        for (int i = 0; i < 3; i++)
        {
            Assert.True(licencija.TryUseDownload());
        }

        licencija.ChangeDownloadLimit(5);

        Assert.Equal(3, licencija.DownloadCount);

        bool allowed = licencija.TryUseDownload();

        Assert.True(allowed);
        Assert.Equal(4, licencija.DownloadCount);
        Assert.Equal(5, licencija.DownloadLimit);
    }

    // Limito pakeitimas neturi aktyvuoti sustabdytos licencijos.
    [Fact]
    public void ChangeDownloadLimit_DoesNotActivateSuspendedLicense()
    {
        License licencija = new License(1, 15, 3, true);
        Assert.True(licencija.TryUseDownload());
        licencija.Suspend();

        licencija.ChangeDownloadLimit(10);

        Assert.Equal(10, licencija.DownloadLimit);
        Assert.Equal(1, licencija.DownloadCount);
        Assert.False(licencija.IsActive);
        Assert.False(licencija.TryUseDownload());
        Assert.Equal(1, licencija.DownloadCount);
    }

    // Netinkamas pakeitimas turi sukelti išimtį
    // ir išsaugoti ankstesnius duomenis.
    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void ChangeDownloadLimit_NegativeValue_ThrowsAndPreservesState(
        int newLimit)
    {
        License licencija = new License(1, 15, 3, true);
        Assert.True(licencija.TryUseDownload());

        Assert.Throws<ArgumentOutOfRangeException>(
            () => licencija.ChangeDownloadLimit(newLimit));

        Assert.Equal(3, licencija.DownloadLimit);
        Assert.Equal(1, licencija.DownloadCount);
        Assert.True(licencija.IsActive);
    }
}