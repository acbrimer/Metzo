namespace Metzo.Demo.DbContexts.WholesaleSodaDbContext;

using System.ComponentModel.DataAnnotations;

public class Address
{
    public int AddressId { get; set; }

    [Required]
    public string Street { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string State { get; set; }

    [Required]
    [RegularExpression(
        @"^\d{5}(-\d{4})?$",
        ErrorMessage = "Invalid ZIP code format. Use '12345' or '12345-6789'."
    )]
    public string PostalCode { get; set; }

    [Range(-90.0, 90.0, ErrorMessage = "Latitude must be between -90 and 90.")]
    public double Latitude { get; set; }

    [Range(-180.0, 180.0, ErrorMessage = "Longitude must be between -180 and 180.")]
    public double Longitude { get; set; }

    public bool IsValid { get; set; }

    public bool IsBlacklisted { get; set; }

    // Optional subunit for apartment number, suite, etc.
    public string Subunit { get; set; }

    // Navigation property
    public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
    public virtual ICollection<Order> BilledOrders { get; set; }
    public virtual ICollection<OrderShippingAddress> ShippedOrders { get; set; }
}
