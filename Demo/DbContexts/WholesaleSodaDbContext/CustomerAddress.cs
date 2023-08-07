namespace Metzo.Demo.DbContexts.WholesaleSodaDbContext;

public class CustomerAddress
{
    public int CustomerAccountId { get; set; }
    public int AddressId { get; set; }
    public bool DefaultShippingAddress { get; set; }
    public bool DefaultBillingAddress { get; set; }
    public int CustomerId { get; set; }
    public virtual Address Address { get; set; }
    public virtual Customer Customer { get; set; }
}
