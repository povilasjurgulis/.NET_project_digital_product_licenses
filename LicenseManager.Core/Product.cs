using System;

public class Product
{
    public int Id{ get; set;}
    public string Name{ get; set; }
    public decimal Price{ get; set; }

    public Product( int newId, string newName, decimal newPrice)
    {
        if(newId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newId), "Produkto Id turi būti teigiamas!");
        }

        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("Produkto pavadinimas negali būti tuščias!", nameof(newName));
        }

        if(newPrice < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newPrice), "Produkto kaina negali būti neigiama!");
        }
        
        Id = newId;
        Name =newName;
        Price = newPrice;
    }

    private Product() // šis konstruktorius leidžia EF Core sukurti objektą skaitant duomenis ir užpildyti jo savybes iš duomenų bazės
    {
        Name = string.Empty;
    }
}