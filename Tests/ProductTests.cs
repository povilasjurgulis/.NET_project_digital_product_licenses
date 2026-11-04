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
}