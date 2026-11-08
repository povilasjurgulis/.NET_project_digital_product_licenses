using System;

public class OrderItem
{
    public int OrderItemId { get; private set; }
    public decimal PriceAtTheTime { get; private set; }
    public int ProductId { get; private set; }
    public int OrderId { get; private set; }

    public OrderItem(int newProductId, decimal newPriceAtTheTime)
    {
        if(newProductId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(newProductId),
                "Produkto ID turi būti didesnis už nulį.");
        }

        if(newPriceAtTheTime < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(newPriceAtTheTime),
                "Kaina negali būti neigiama.");
        }

        ProductId = newProductId;
        PriceAtTheTime = newPriceAtTheTime;

        // OrderItemId suteiks duomenų bazė.
    }

    // EF Core naudos šį konstruktorių duomenims nuskaityti.
    private OrderItem()
    {
    }
}
