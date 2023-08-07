namespace Metzo.Demo.DbContexts.WholesaleSodaDbContext;
public class OrderShippingAddress
{
    public int OrderShippingAddressId { get; set; }
    public int OrderId { get; set; }
    public int AddressId { get; set; }
    public int? OrderProductId { get; set; }
    public virtual Order Order { get; set; }
    public virtual OrderProduct? OrderProduct { get; set; }
    public virtual Address Address { get; set; }
}
