using Microsoft.EntityFrameworkCore;
using Metzo.Core;
using Metzo.Core.Filters;
using Metzo.Demo.DbContexts.WholesaleSodaDbContext;

using (var db = new WholesaleSodaContext(new DbContextOptions<WholesaleSodaContext>()))
{
    var customersQuery1 = db.Set<Customer>().WithFilters(".");

    var filterCustomers1 = new Dictionary<string, FilterGroup> {
        {".", new FilterGroup(
            new FilterPredicate("FirstName", "Dave"),
            new FilterPredicate("LastName", "Smith")
        )}
    };
    var visitor1 = new WithFiltersExpressionVisitor(filterCustomers1);
    var customersQuery1_filtered = visitor1.GetFilteredQuery(customersQuery1);
    Console.WriteLine(customersQuery1_filtered.ToQueryString());

    // `.WithFilters` can be used in subqueries
    // This is kind of the whole point as it allows for dynamic filtering in underlying aggregations
    var ordersSubquery = db.Set<Order>().WithFilters("orders").GroupBy(k => k.CustomerId).Select(g => new
    {
        CustomerId = g.Key,
        CountOrders = g.Count(),
        OrdersTotalSpend = g.Sum(r => r.Total),
        OrdersAvgSpend = g.Average(r => r.Total)
    });
    var customersQuery2 = db.Set<Customer>().WithFilters(".").Join(
        ordersSubquery,
        customer => customer.CustomerId,
        order => order.CustomerId,
        (customer, orderTotals) => new
        {
            customer,
            orderTotals
        }
    ).Join(
        db.Set<CustomerAddress>(),
        i => i.customer.CustomerId,
        customerAddresses => customerAddresses.CustomerId,
        (i, ca) => new
        {
            i.customer,
            i.orderTotals,
            address = ca.Address,
            ca.Address.IsBlacklisted,
            ca.Address.IsValid
        }
    ).WithFilters("addresses").Select(r => new
    {
        Id = r.customer.CustomerId,
        FirstName = r.customer.FirstName,
        LastName = r.customer.LastName,
        CountOrders = r.orderTotals.CountOrders,
        OrdersTotalSpend = r.orderTotals.OrdersTotalSpend,
        OrdersAvgSpend = r.orderTotals.OrdersAvgSpend,
    });

    // Again, calling `.ToQueryString()`, `.ToList()`, etc should ignore `.WithFilters`
    Console.WriteLine("customersQuery2:");
    Console.WriteLine("------------------------------");
    Console.WriteLine("- customersQuery2.ToQueryString():");
    // Console.WriteLine(customersQuery2.ToQueryString());
    var filterCustomers2 = new Dictionary<string, FilterGroup> {
        {".", new FilterGroup(
            new FilterPredicate("FirstName", "Dave"),
            new FilterPredicate("LastName", "Smith")
        )},
        {
            "orders", new FilterGroup(
                new FilterPredicate("OrderYear", 2020)
            )
        },

    };
    var visitor2 = new WithFiltersExpressionVisitor(filterCustomers2);
    var customersQuery2_filtered = visitor2.GetFilteredQuery(customersQuery2);
    Console.WriteLine("- customersQuery2_filtered.ToQueryString():");
    Console.WriteLine(customersQuery2_filtered.ToQueryString());
    // var customersQuery2_WithFilters = visitor1.GetFilteredQuery(customersQuery1, filterCustomers1);
    // Console.WriteLine("- customersQuery2_WithFilters.ToQueryString():");
    // Console.WriteLine(customersQuery2_WithFilters.ToQueryString());
}