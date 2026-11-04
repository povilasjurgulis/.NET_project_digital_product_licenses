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

    // Netinka nei null, nei tuščias tekstas, nei vien tarpai
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void Name_CannotBe_Null_Or_WhiteSpace(string? newName) // ? reiškia, kad gali būti null reikšmė
    {
        Assert.Throws<ArgumentException>(
            () => new Product(4, newName!, 47m)); // ! yra null forgiving operator (neperspėja kompiliatoriaus apie null)
    }

    // Neigiama kaina neleidžiama
    [Theory]
    [InlineData(-1)]
    [InlineData(-11550880)]
    public void NegativePrice_NotAllowed(int newPrice)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Product(7, "ShakeFX", newPrice));
    }
}