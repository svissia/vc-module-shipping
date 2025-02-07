using System.Reflection;
using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.ShippingModule.Data.Model;

namespace VirtoCommerce.ShippingModule.Data.Repositories
{
    public class ShippingDbContext : DbContextWithTriggers
    {
        public ShippingDbContext(DbContextOptions<ShippingDbContext> options) : base(options)
        {
        }

        protected ShippingDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoreShippingMethodEntity>().ToTable("StoreShippingMethod").HasKey(x => x.Id);
            modelBuilder.Entity<StoreShippingMethodEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<StoreShippingMethodEntity>().Property(x => x.StoreId).HasMaxLength(128);
            modelBuilder.Entity<StoreShippingMethodEntity>().Property(x => x.TypeName).HasMaxLength(128);
            modelBuilder.Entity<StoreShippingMethodEntity>().HasIndex(x => new { x.TypeName, x.StoreId })
                .HasDatabaseName("IX_StoreShippingMethodEntity_TypeName_StoreId")
                .IsUnique();

            base.OnModelCreating(modelBuilder);

            // Allows configuration for an entity type for different database types.
            // Applies configuration from all <see cref="IEntityTypeConfiguration{TEntity}" in VirtoCommerce.ShippingModule.Data.XXX project. /> 
            switch (this.Database.ProviderName)
            {
                case "Pomelo.EntityFrameworkCore.MySql":
                    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.ShippingModule.Data.MySql"));
                    break;
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.ShippingModule.Data.PostgreSql"));
                    break;
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.ShippingModule.Data.SqlServer"));
                    break;
            }

        }
    }
}
