namespace Metzo.Demo.DbContexts.WholesaleSodaDbContext;

using System.ComponentModel.DataAnnotations;
public class Product
{
    [MaxLength(12)]
    public string ProductCode { get; set; }
    public string Name { get; set; }
    public decimal PricePerLiter { get; set; }
    public double CaloriesPerMl { get; set; }
    public double SugarPerMl { get; set; }
    public double CaffeinePerMl { get; set; }
    public int ProductLineId { get; set; }
    public virtual ProductLine ProductLine { get; set; }
    public virtual ICollection<OrderProduct> ProductOrders { get; set; }
}
