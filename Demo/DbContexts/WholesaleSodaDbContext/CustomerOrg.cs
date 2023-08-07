namespace Metzo.Demo.DbContexts.WholesaleSodaDbContext;

public class CustomerOrg
{
    public int CustomerOrgId { get; set; }
    public string Name { get; set; }
    public string? Bio { get; set; }
    public string? ProfileImageURL { get; set; }
    public List<Customer> Customers { get; set; }
}
