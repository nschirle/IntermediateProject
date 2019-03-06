using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalogAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogAPI.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<CatalogType> CatalogTypes { get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogItem> CatalogItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatalogType>(configureCatalogType);
            modelBuilder.Entity<CatalogBrand>(configureCatalogBrand);
            modelBuilder.Entity<CatalogItem>(configureCatalogItem);
        }

        private void configureCatalogItem(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.ToTable("Catalog");
            builder.Property(c => c.Id).IsRequired().ForSqlServerUseSequenceHiLo("catalog_hilo");
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Price).IsRequired();
            builder.Property(c => c.Url).IsRequired(false);
            builder.HasOne(c => c.CatalogType).WithMany().HasForeignKey(c => c.CatalogTypeId);
            builder.HasOne(c => c.CatalogBrand).WithMany().HasForeignKey(c => c.CatalogBrandId);

        }

        private void configureCatalogType(EntityTypeBuilder<CatalogType> builder)
        {
            builder.ToTable("CatalogTypes");
            builder.Property(c => c.Id).IsRequired().ForSqlServerUseSequenceHiLo("catalog_types_hilo");
            builder.Property(c => c.Type).IsRequired().HasMaxLength(50);

        }
        private void configureCatalogBrand(EntityTypeBuilder<CatalogBrand> builder)
        {
            builder.ToTable("CatalogBrand");
            builder.Property(c => c.Id).IsRequired().ForSqlServerUseSequenceHiLo("catalog_Brands_hilo");
            builder.Property(c => c.Brand).IsRequired().HasMaxLength(50);

        }

    }
}
