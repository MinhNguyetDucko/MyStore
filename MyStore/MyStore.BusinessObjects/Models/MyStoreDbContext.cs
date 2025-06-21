using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MyStore.BusinessObjects.Models
{
    public partial class MyStoreDbContext : DbContext
    {
        public MyStoreDbContext()
        {
        }

        public MyStoreDbContext(DbContextOptions<MyStoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountMember> AccountMembers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        private string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            return configuration["ConnectionStrings:DefaultConnectionString"];
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(GetConnectionString());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // AccountMember configuration
            modelBuilder.Entity<AccountMember>(entity =>
            {
                entity.HasKey(e => e.MemberId);
                entity.ToTable("accountmember");
                entity.Property(e => e.MemberId)
                    .HasColumnName("memberid")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.MemberPassword).HasColumnName("memberpassword").IsRequired();
                entity.Property(e => e.FullName).HasColumnName("fullname");
                entity.Property(e => e.EmailAddress).HasColumnName("emailaddress");
                entity.Property(e => e.MemberRole).HasColumnName("memberrole");
            });

            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.ToTable("categories");
                entity.Property(e => e.CategoryId)
                    .HasColumnName("categoryid")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.CategoryName).HasColumnName("categoryname").IsRequired();
            });

            // Product configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);
                entity.ToTable("products");
                entity.Property(e => e.ProductId)
                    .HasColumnName("productid")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.ProductName).HasColumnName("productname").IsRequired();
                entity.Property(e => e.CategoryId).HasColumnName("categoryid");
                entity.Property(e => e.UnitsInStock).HasColumnName("unitsinstock");
                entity.Property(e => e.UnitPrice).HasColumnName("unitprice").HasColumnType("decimal(10,2)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}