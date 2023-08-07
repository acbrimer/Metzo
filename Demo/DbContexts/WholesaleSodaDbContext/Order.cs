namespace Metzo.Demo.DbContexts.WholesaleSodaDbContext;

public class Order
{
    public int OrderId { get; set; }

    public Guid TransactionId { get; set; }
    public decimal Total { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }

    // Navigation properties
    public int BillingAddressId { get; set; }
    public int CustomerId { get; set; }

    public string OrderDate { get; set; }
    public int OrderYear { get; set; }
    public virtual Address BillingAddress { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    public virtual ICollection<OrderShippingAddress> OrderShippingAddresses { get; set; }
}
