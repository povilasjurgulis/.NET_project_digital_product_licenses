using System.Net.ServerSentEvents;
using System.Text;

public class Order
{
    public int OrderId { get; private set;}
    public string ClientId {get; private set;}
    public DateTime OrderCreationDate {get; private set;}
    public OrderPaymentStatus PaymentStatus {get; private set;}
    public List <OrderItem> Items {get; } = new();

    public Order(int newOrderId, string newClientId){
        if(newOrderId < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newOrderId), "Užsakymo Id negali būti neigiamas!");
        }
        
        if(string.IsNullOrWhiteSpace(newClientId))
        {
            throw new ArgumentException("Produkto pavadinimas negali būti tuščias!", nameof(newClientId));
        }

        OrderId = newOrderId;
        ClientId = newClientId;
        OrderCreationDate = DateTime.UtcNow;
        PaymentStatus = OrderPaymentStatus.Pending;
    }

    private Order()
    {
        ClientId = string.Empty;
    }

    public void AddItem(OrderItem item)
    {
        if(item.OrderItemId.ToString() == null)
        {
            throw new ArgumentException("Užsakymo produkto ID negali būti tuščias!", nameof(AddItem));
        }

        while(PaymentStatus == OrderPaymentStatus.Pending)
        {
            Items.Append(item);
        }
    }

    public void MarkAsPaid()
    {
        if(PaymentStatus == OrderPaymentStatus.Pending && Items.Count > 0)
        {
            PaymentStatus = OrderPaymentStatus.Paid;
        }
    }

    public void Cancel()
    {
        if(PaymentStatus == OrderPaymentStatus.Pending)
        {
            PaymentStatus = OrderPaymentStatus.Cancelled;
        }
    }
}