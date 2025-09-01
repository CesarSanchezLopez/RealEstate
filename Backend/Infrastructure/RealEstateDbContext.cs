using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure
{
    public class RealEstateDbContext : DbContext
    {
        public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options)
            : base(options) { }

        public DbSet<Owner> Owners { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<PropertyTrace> PropertyTraces { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ========== Owner ==========
            modelBuilder.Entity<Owner>(entity =>
            {
                entity.HasKey(o => o.IdOwner);
                entity.Property(o => o.Name)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.Property(o => o.Address)
                      .HasMaxLength(250);
                entity.Property(o => o.Photo)
                      .HasMaxLength(500);
            });

            // ========== Property ==========
            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(p => p.IdProperty);
                entity.Property(p => p.Name)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.Property(p => p.Address)
                      .HasMaxLength(250);
                entity.Property(p => p.CodeInternal)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(p => p.Price)
                      .HasColumnType("decimal(18,2)");

                // Relación Owner -> Property (1:N)
                entity.HasOne(p => p.Owner)
                      .WithMany(o => o.Properties)
                      .HasForeignKey(p => p.IdOwner);
            });

            // ========== PropertyImage ==========
            modelBuilder.Entity<PropertyImage>(entity =>
            {
                entity.HasKey(pi => pi.IdPropertyImage);

                entity.Property(pi => pi.File)
                      .IsRequired()
                      .HasColumnType("varbinary(max)");  // ahora binario

                entity.HasOne(pi => pi.Property)
                      .WithMany(p => p.Images)
                      .HasForeignKey(pi => pi.IdProperty);
            });

            // ========== PropertyTrace ==========
            modelBuilder.Entity<PropertyTrace>(entity =>
            {
                entity.HasKey(pt => pt.IdPropertyTrace);
                entity.Property(pt => pt.Value)
                      .HasColumnType("decimal(18,2)");
                entity.Property(pt => pt.Tax)
                      .HasColumnType("decimal(18,2)");
                entity.Property(pt => pt.DateSale)
                      .IsRequired();

                // Relación Property
                entity.HasOne(pt => pt.Property)
                      .WithMany(p => p.Traces)
                      .HasForeignKey(pt => pt.IdProperty);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
