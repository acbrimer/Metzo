namespace Metzo.Demo.DbContexts.WholesaleSodaDbContext;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Bogus;

public static class SeedData
{
    public static void SeedProductLines(ModelBuilder modelBuilder)
    {
        List<ProductLine> productLines = new List<ProductLine>
        {
            new ProductLine { ProductLineId = 1, Name = "Coke" },
            new ProductLine { ProductLineId = 2, Name = "Pepsi" },
            new ProductLine { ProductLineId = 3, Name = "Sprite" },
            new ProductLine { ProductLineId = 4, Name = "Dr. Pepper" }
            // Add more product lines if needed
        };

        modelBuilder.Entity<ProductLine>().HasData(productLines);
    }

    public static void SeedProducts(ModelBuilder modelBuilder)
    {
        List<Product> products = new List<Product>
        {
            // Coke products
            new Product { ProductCode = "COK-C", Name = "Coke", PricePerLiter = 1.5m, CaloriesPerMl = 0.4, SugarPerMl = 0.1, CaffeinePerMl = 0.03, ProductLineId = 1 },
            new Product { ProductCode = "COK-DC", Name = "Diet Coke", PricePerLiter = 1.3m, CaloriesPerMl = 0.1, SugarPerMl = 0, CaffeinePerMl = 0.03, ProductLineId = 1 },
            new Product { ProductCode = "COK-CZ", Name = "Coke Zero", PricePerLiter = 1.3m, CaloriesPerMl = 0, SugarPerMl = 0, CaffeinePerMl = 0.03, ProductLineId = 1 },
            new Product { ProductCode = "COK-CC", Name = "Cherry Coke", PricePerLiter = 1.6m, CaloriesPerMl = 0.45, SugarPerMl = 0.12, CaffeinePerMl = 0.03, ProductLineId = 1 },
            new Product { ProductCode = "COK-DCC", Name = "Diet Cherry Coke", PricePerLiter = 1.4m, CaloriesPerMl = 0.15, SugarPerMl = 0.02, CaffeinePerMl = 0.03, ProductLineId = 1 },

            // Pepsi products
            new Product { ProductCode = "PEP-P", Name = "Pepsi", PricePerLiter = 1.4m, CaloriesPerMl = 0.38, SugarPerMl = 0.09, CaffeinePerMl = 0.03, ProductLineId = 2 },
            new Product { ProductCode = "PEP-DP", Name = "Diet Pepsi", PricePerLiter = 1.2m, CaloriesPerMl = 0.1, SugarPerMl = 0, CaffeinePerMl = 0.03, ProductLineId = 2 },
            new Product { ProductCode = "PEP-PM", Name = "Pepsi Max", PricePerLiter = 1.2m, CaloriesPerMl = 0, SugarPerMl = 0, CaffeinePerMl = 0.03, ProductLineId = 2 },
            // Add more Pepsi products as needed

            // Sprite products
            new Product { ProductCode = "SPR-S", Name = "Sprite", PricePerLiter = 1.3m, CaloriesPerMl = 0.38, SugarPerMl = 0.09, CaffeinePerMl = 0, ProductLineId = 3 },
            new Product { ProductCode = "SPR-LS", Name = "Sprite Zero", PricePerLiter = 1.2m, CaloriesPerMl = 0, SugarPerMl = 0, CaffeinePerMl = 0, ProductLineId = 3 },
            // Add more Sprite products as needed

            // Dr. Pepper products
            new Product { ProductCode = "DRP-P", Name = "Dr. Pepper", PricePerLiter = 1.4m, CaloriesPerMl = 0.38, SugarPerMl = 0.09, CaffeinePerMl = 0.03, ProductLineId = 4 },
            new Product { ProductCode = "DRP-DP", Name = "Diet Dr. Pepper", PricePerLiter = 1.2m, CaloriesPerMl = 0.1, SugarPerMl = 0, CaffeinePerMl = 0.03, ProductLineId = 4 },
            // Add more Dr. Pepper products as needed
        };

        modelBuilder.Entity<Product>().HasData(products);
    }

    public static List<CustomerOrg> SeedCustomerOrgs(ModelBuilder modelBuilder)
    {
        // Existing product lines seed data (from previous example)
        // ...

        // Add the "(No Org)" option for CustomerOrg
        List<CustomerOrg> customerOrgs = new List<CustomerOrg>
        {
            new CustomerOrg { CustomerOrgId = 1, Name = "(No Org)", Bio = "No organization", ProfileImageURL = null },
            new CustomerOrg { CustomerOrgId = 2, Name = "Org 1", Bio = "Description for Org 1", ProfileImageURL = null },
            new CustomerOrg { CustomerOrgId = 3, Name = "Org 2", Bio = "Description for Org 2", ProfileImageURL = null },
            // Add more organizations as needed
        };

        modelBuilder.Entity<CustomerOrg>().HasData(customerOrgs);
        return customerOrgs;
    }

    public static List<Customer> SeedCustomers(ModelBuilder modelBuilder)
    {
        // Create 15 fake customers for the 3 CustomerOrgs
        List<Customer> customers = new List<Customer>
        {
            // Customers in "(No Org)"
            new Customer
            {
                CustomerId = 1,
                UserId = Guid.NewGuid(),
                UserName = "user1",
                FirstName = "John",
                LastName = "Doe",
                DisplayName = "John Doe",
                Email = "john.doe@example.com",
                MobilePhone = "123-456-7890",
                CustomerOrgId = 1 // "(No Org)"
            },
            new Customer
            {
                CustomerId = 2,
                UserId = Guid.NewGuid(),
                UserName = "user2",
                FirstName = "Jane",
                LastName = "Smith",
                DisplayName = "Jane Smith",
                Email = "jane.smith@example.com",
                MobilePhone = "111-222-3333",
                CustomerOrgId = 1 // "(No Org)"
            },
            // Add more customers for "(No Org)" if needed

            // Customers in "Org 1"
            new Customer
            {
                CustomerId = 3,
                UserId = Guid.NewGuid(),
                UserName = "user3",
                FirstName = "Robert",
                LastName = "Johnson",
                DisplayName = "Robert Johnson",
                Email = "robert.johnson@example.com",
                MobilePhone = "999-888-7777",
                CustomerOrgId = 2 // Org 1
            },
            new Customer
            {
                CustomerId = 4,
                UserId = Guid.NewGuid(),
                UserName = "user4",
                FirstName = "Lisa",
                LastName = "Anderson",
                DisplayName = "Lisa Anderson",
                Email = "lisa.anderson@example.com",
                MobilePhone = "444-555-6666",
                CustomerOrgId = 2 // Org 1
            },
            // Add more customers for "Org 1" if needed

            // Customers in "Org 2"
            new Customer
            {
                CustomerId = 5,
                UserId = Guid.NewGuid(),
                UserName = "user5",
                FirstName = "Michael",
                LastName = "Brown",
                DisplayName = "Michael Brown",
                Email = "michael.brown@example.com",
                MobilePhone = "777-888-9999",
                CustomerOrgId = 3 // Org 2
            },
            new Customer
            {
                CustomerId = 6,
                UserId = Guid.NewGuid(),
                UserName = "user6",
                FirstName = "Emily",
                LastName = "Johnson",
                DisplayName = "Emily Johnson",
                Email = "emily.johnson@example.com",
                MobilePhone = "111-222-3333",
                CustomerOrgId = 3 // Org 2
            },
        };

        modelBuilder.Entity<Customer>().HasData(customers);
        return customers;
    }

    public static List<Address> SeedAddresses(ModelBuilder modelBuilder)
    {
        var addressFaker = new Faker<Address>()
            .RuleFor(a => a.AddressId, f => f.IndexFaker + 1)
            .RuleFor(a => a.Street, f => f.Address.StreetAddress())
            .RuleFor(a => a.City, f => f.Address.City())
            .RuleFor(a => a.State, f => f.Address.StateAbbr())
            .RuleFor(a => a.PostalCode, f => f.Address.ZipCode("#####"))
            .RuleFor(a => a.Latitude, f => f.Address.Latitude())
            .RuleFor(a => a.Longitude, f => f.Address.Longitude())
            .RuleFor(a => a.IsValid, true)
            .RuleFor(a => a.IsBlacklisted, false)
            .RuleFor(a => a.Subunit, f => f.Random.Bool(0.3f) ? f.Address.SecondaryAddress() : null);

        List<Address> addresses = addressFaker.Generate(10);

        modelBuilder.Entity<Address>().HasData(addresses);

        return addresses;
    }

    public static void SeedCustomerAddresses(ModelBuilder modelBuilder)
    {
        // Retrieve the customer IDs from the existing SeedCustomers method
        List<Customer> customers = SeedCustomers(modelBuilder);
        int[] customerIds = customers.Select(c => c.CustomerId).ToArray();

        // Retrieve the address IDs from the existing SeedAddresses method
        List<Address> addresses = SeedAddresses(modelBuilder);
        int[] addressIds = addresses.Select(a => a.AddressId).ToArray();

        var customerAddressFaker = new Faker<CustomerAddress>()
            .RuleFor(ca => ca.CustomerAccountId, f => f.PickRandom(customerIds))
            .RuleFor(ca => ca.AddressId, f => f.PickRandom(addressIds))
            .RuleFor(ca => ca.DefaultShippingAddress, f => f.Random.Bool())
            .RuleFor(ca => ca.DefaultBillingAddress, f => f.Random.Bool());

        List<CustomerAddress> customerAddresses = customerAddressFaker.Generate(15); // Adjust the count as needed

        modelBuilder.Entity<CustomerAddress>().HasData(customerAddresses);
    }


}
