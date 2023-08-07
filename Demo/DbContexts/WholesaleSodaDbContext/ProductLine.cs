namespace Metzo.Demo.DbContexts.WholesaleSodaDbContext;

public class ProductLine
{
    public int ProductLineId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Product> Products { get; set; }
}
