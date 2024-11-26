using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SQL.Ents;
using SQL.Extensions;

namespace SQL;

public class SalesDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public DbSet<Store> Stores { get; set; }

    public DbSet<Sale> Sales { get; set; }

    public DbSet<SaleItem> SaleItems { get; set; }
    
   

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Replace with your actual connection string
        optionsBuilder.UseNpgsql(@"Server=127.0.0.1;Port=5432;Database=StoreTask;User Id=postgres;Password=mysecretpassword;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyAllConfigurations();
    }
}