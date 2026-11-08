public class OrderItem
{
    public int OrderItemId {get; private set;}
    public decimal PriceAtTheTime {get; private set;}
    public int ProductId {get; private set;}
    public int OrderId {get; private set;}

    public OrderItem(int newOrderItemId, decimal newPriceAtTheTime, int newProductId, int newOrderId)
    {
        if(newOrderItemId < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newOrderItemId), "Užsakymo produkto Id negali būti neigiamas!");
        }

        if(newPriceAtTheTime < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newPriceAtTheTime), "Kaina negali būti neigiama!");
        }

        if(newProductId < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newProductId), "Produkto Id negali būti neigiamas!");
        }


        OrderItemId = newOrderItemId;
        PriceAtTheTime = newPriceAtTheTime;
        ProductId = newProductId;
    }

    private OrderItem()
    {
    }
}