namespace Metzo.Demo.DbContexts.WholesaleSodaDbContext;

public class OrderProduct
{
    public int OrderProductId { get; set; }
    public int OrderId { get; set; }
    public string ProductCode { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }

    // Navigation properties
    public virtual Order Order { get; set; }
    public virtual Product Product { get; set; }
    public virtual OrderShippingAddress? ShippingAddress { get; set; }
}
