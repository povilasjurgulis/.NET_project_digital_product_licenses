using System;
using System.Collections.Generic;

public class Order
{
    private readonly List<OrderItem> _items = new();

    public int OrderId { get; private set; }
    public string ClientId { get; private set; }
    public DateTime OrderCreationDate { get; private set; }
    public OrderPaymentStatus PaymentStatus { get; private set; }

    // Sąrašą galima peržiūrėti, o eilutes prideda AddItem().
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

    public Order(string newClientId)
    {
        if (string.IsNullOrWhiteSpace(newClientId))
        {
            throw new ArgumentException(
                "Kliento paskyros ID negali būti tuščias.",
                nameof(newClientId));
        }

        ClientId = newClientId;
        OrderCreationDate = DateTime.UtcNow;
        PaymentStatus = OrderPaymentStatus.Pending;

        // OrderId suteiks duomenų bazė.
    }

    // EF Core naudos šį konstruktorių duomenims nuskaityti.
    private Order()
    {
        ClientId = string.Empty;
    }

    public void AddItem(OrderItem item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        EnsurePending();

        if (_items.Contains(item))
        {
            throw new InvalidOperationException(
                "Ši užsakymo eilutė jau pridėta.");
        }

        _items.Add(item);
    }

    public void MarkAsPaid()
    {
        EnsurePending();

        if (_items.Count == 0)
        {
            throw new InvalidOperationException(
                "Tuščio užsakymo negalima pažymėti apmokėtu.");
        }

        PaymentStatus = OrderPaymentStatus.Paid;
    }

    public void Cancel()
    {
        EnsurePending();
        PaymentStatus = OrderPaymentStatus.Cancelled;
    }

    private void EnsurePending()
    {
        if (PaymentStatus != OrderPaymentStatus.Pending)
        {
            throw new InvalidOperationException(
                "Šis veiksmas leidžiamas tik apmokėjimo laukiančiam užsakymui.");
        }
    }
}
