namespace Metzo.Demo.DbContexts.WholesaleSodaDbContext;

public class Customer
{
    public int CustomerId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string? MobilePhone { get; set; }
    public string? WorkPhone { get; set; }
    public string? Fax { get; set; }
    // Navigation properties
    public int CustomerOrgId { get; set; }
    public virtual CustomerOrg CustomerOrg { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<CustomerAddress> CustomerAddresseses { get; set; }
}
