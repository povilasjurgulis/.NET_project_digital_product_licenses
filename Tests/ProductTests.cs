using System;
using Xunit;

public class ProductTests
{
    // Teisingi duomenys turi būti išsaugoti nepakeisti
    [Fact]
    public void Constructor_Correctly_SavesData()
    {
        Product produktas = new Product(2, "DistortedFX", 76.99m);

        Assert.Equal(2, produktas.Id);
        Assert.Equal("DistortedFX", produktas.Name);
        Assert.Equal(76.99m, produktas.Price);
    }

    // Produktas gali būti nemokamas
    [Fact]
    public void Product_CanBeFree()
    {
        Product produktas = new Product(3, "FreeFX", 0m);

        Assert.Equal(0m, produktas.Price);
    }

    // Produkto ID turi būti teigiamas
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ProductID_MustBePositive(int newProductId)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Product(newProductId, "Shake Presets", 47m));
    }
}