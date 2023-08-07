namespace Metzo.Demo.DbContexts.WholesaleSodaDbContext;

using Microsoft.EntityFrameworkCore;

public class WholesaleSodaContext : DbContext
{
    // Constructor to configure the DbContext options
    public WholesaleSodaContext(DbContextOptions<WholesaleSodaContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Replace 'YourConnectionString' with the actual connection string to your SQLite database file.
        optionsBuilder.UseSqlServer(@"Data Source=any.db;");
    }

    // Entities representing the database tables
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerOrg> CustomerOrgs { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<OrderShippingAddress> OrderShippingAddresses { get; set; }
    public DbSet<CustomerAddress> CustomerAddresses { get; set; }
    public DbSet<ProductLine> ProductLines { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure primary keys
        modelBuilder.Entity<Customer>().HasKey(c => c.CustomerId);
        modelBuilder.Entity<CustomerOrg>().HasKey(c => c.CustomerOrgId);
        modelBuilder.Entity<Address>().HasKey(a => a.AddressId);
        modelBuilder.Entity<Product>().HasKey(p => p.ProductCode);
        modelBuilder.Entity<ProductLine>().HasKey(p => p.ProductLineId);
        modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
        modelBuilder.Entity<OrderProduct>().HasKey(op => op.OrderProductId);
        modelBuilder.Entity<OrderShippingAddress>().HasKey(osa => osa.OrderShippingAddressId);
        modelBuilder.Entity<CustomerAddress>().HasKey(caa => new { caa.CustomerAccountId, caa.AddressId });

        // Configure relationships
        modelBuilder
            .Entity<Customer>()
            .HasOne(c => c.CustomerOrg)
            .WithMany(co => co.Customers)
            .HasForeignKey(c => c.CustomerOrgId)
            .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.Cascade if you want to delete the organization along with its accounts

        modelBuilder
            .Entity<Customer>()
            .HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<Customer>()
            .HasMany(c => c.CustomerAddresseses)
            .WithOne(caa => caa.Customer)
            .HasForeignKey(c => c.CustomerAccountId)
            .OnDelete(DeleteBehavior.Cascade); // or DeleteBehavior.Restrict if you want to keep the addresses when an account is deleted

        modelBuilder
            .Entity<Order>()
            .HasOne(o => o.BillingAddress)
            .WithMany(a => a.BilledOrders)
            .HasForeignKey(o => o.BillingAddressId)
            .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.Cascade if you want to delete the billing address along with the order

        modelBuilder
            .Entity<Order>()
            .HasMany(o => o.OrderShippingAddresses)
            .WithOne(osa => osa.Order)
            .HasForeignKey(o => o.OrderId)
            .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.Cascade if you want to delete the billing address along with the order

        modelBuilder
            .Entity<Order>()
            .HasMany(o => o.OrderProducts)
            .WithOne(op => op.Order)
            .HasForeignKey(o => o.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<OrderShippingAddress>()
            .HasOne(osa => osa.OrderProduct)
            .WithOne(op => op.ShippingAddress)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<OrderShippingAddress>()
            .HasOne(osa => osa.Address)
            .WithMany(a => a.ShippedOrders)
            .HasForeignKey(osa => osa.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<OrderProduct>()
            .HasOne(op => op.Product)
            .WithMany(p => p.ProductOrders)
            .HasForeignKey(osa => osa.ProductCode)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<Product>()
            .HasOne(p => p.ProductLine)
            .WithMany(pl => pl.Products)
            .HasForeignKey(p => p.ProductLineId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for performance optimization
        modelBuilder.Entity<Customer>().HasIndex(c => c.UserName).IsUnique();
        modelBuilder.Entity<Product>().HasIndex(p => p.Name);
        modelBuilder.Entity<Order>().HasIndex(o => o.TransactionId).IsUnique();
    }
}

// The rest of your entity classes go here...
