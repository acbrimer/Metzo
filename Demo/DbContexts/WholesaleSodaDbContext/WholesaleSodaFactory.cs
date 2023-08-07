namespace Metzo.Demo.DbContexts.WholesaleSodaDbContext;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

public class WholesaleSodaContextFactory : IDesignTimeDbContextFactory<WholesaleSodaContext>
{
    public WholesaleSodaContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WholesaleSodaContext>();
        optionsBuilder.UseSqlite(@"Data Source=WholesaleSoda.db");

        return new WholesaleSodaContext(optionsBuilder.Options);
    }
}