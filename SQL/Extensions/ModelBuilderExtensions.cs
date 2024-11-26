using Microsoft.EntityFrameworkCore;
using SQL.Ents;

namespace SQL.Extensions;

public static class ModelBuilderExtensions
    {
          private static void ConfigureProduct(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Product>();

            entity.HasKey(p => p.ProductId);

            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(p => p.UnitPrice)
                  .HasColumnType("decimal(18,2)");

            entity.HasMany(p => p.SaleItems)
                  .WithOne(si => si.Product)
                  .HasForeignKey(si => si.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        }

          private static void ConfigureStore(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Store>();

            entity.HasKey(s => s.StoreId);

            entity.Property(s => s.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasMany(s => s.Sales)
                  .WithOne(sale => sale.Store)
                  .HasForeignKey(sale => sale.StoreId)
                  .OnDelete(DeleteBehavior.Cascade);
        }

          private static void ConfigureSale(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Sale>();

            entity.HasKey(s => s.SaleId);

            entity.Property(s => s.SaleDate)
                  .IsRequired();

            entity.HasMany(s => s.SaleItems)
                  .WithOne(si => si.Sale)
                  .HasForeignKey(si => si.SaleId)
                  .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureSaleItem(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<SaleItem>();

            entity.HasKey(si => si.SaleItemId);

            entity.Property(si => si.Quantity)
                  .IsRequired();

            entity.Property(si => si.UnitPrice)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)");

            // TotalPrice is a computed property, so no need to map it to the database
        }

        public static void ApplyAllConfigurations(this ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureProduct();
            modelBuilder.ConfigureStore();
            modelBuilder.ConfigureSale();
            modelBuilder.ConfigureSaleItem();
            
        }
    }