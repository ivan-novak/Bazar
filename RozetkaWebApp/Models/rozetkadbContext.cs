using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class rozetkadbContext : DbContext
    {
        public rozetkadbContext()
        {
        }

        public rozetkadbContext(DbContextOptions<rozetkadbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Catalog> Catalogs { get; set; }
        public virtual DbSet<CatalogImage> CatalogImages { get; set; }
        public virtual DbSet<Characteristic> Characteristics { get; set; }
        public virtual DbSet<ControlImage> ControlImages { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Portal> Portals { get; set; }
        public virtual DbSet<PortalImage> PortalImages { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<View> Views { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:novak-sql.database.windows.net,1433;Initial Catalog=rozetka-db;Persist Security Info=False;User ID=dbadmin;Password=Rozetka2022;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Catalog>(entity =>
            {
                entity.ToTable("Catalog");

                entity.Property(e => e.CatalogId).HasColumnName("CatalogID");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PortalId).HasColumnName("PortalID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Portal)
                    .WithMany(p => p.Catalogs)
                    .HasForeignKey(d => d.PortalId)
                    .HasConstraintName("FK_Catalogs_Portals");
            });

            modelBuilder.Entity<CatalogImage>(entity =>
            {
                entity.ToTable("CatalogImage");

                entity.Property(e => e.CatalogId).HasColumnName("CatalogID");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Catalog)
                    .WithMany(p => p.CatalogImages)
                    .HasForeignKey(d => d.CatalogId)
                    .HasConstraintName("FK_CatalogImages_Catalogs");
            });

            modelBuilder.Entity<Characteristic>(entity =>
            {
                entity.ToTable("Characteristic");

                entity.Property(e => e.CharacteristicId).HasColumnName("CharacteristicID");

                entity.Property(e => e.Dimension).HasMaxLength(50);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.PropertyId).HasColumnName("PropertyID");

                entity.Property(e => e.Value).HasMaxLength(500);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Characteristics)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Characteristics_Products");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.Characteristics)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Characteristics_Properties");
            });

            modelBuilder.Entity<ControlImage>(entity =>
            {
                entity.ToTable("ControlImage");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Quantity).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("UserID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Orders_Products");
            });

            modelBuilder.Entity<Portal>(entity =>
            {
                entity.ToTable("Portal");

                entity.Property(e => e.PortalId).HasColumnName("PortalID");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<PortalImage>(entity =>
            {
                entity.ToTable("PortalImage");

                entity.Property(e => e.PortalImageId).HasColumnName("PortalImageID");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.PortalId).HasColumnName("PortalID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Portal)
                    .WithMany(p => p.PortalImages)
                    .HasForeignKey(d => d.PortalId)
                    .HasConstraintName("FK_PortalImages_Portals");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CatalogId).HasColumnName("CatalogID");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.Quantity).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Catalog)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CatalogId)
                    .HasConstraintName("FK_Products_Catalogs");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.ToTable("ProductImage");

                entity.Property(e => e.ProductImageId).HasColumnName("ProductImageID");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductImages_Products");
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.ToTable("Property");

                entity.Property(e => e.PropertyId).HasColumnName("PropertyID");

                entity.Property(e => e.CatalogId).HasColumnName("CatalogID");

                entity.Property(e => e.Category).HasMaxLength(50);

                entity.Property(e => e.Format).HasMaxLength(50);

                entity.Property(e => e.IsNumber).HasColumnName("isNumber");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Mask).HasMaxLength(50);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Catalog)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.CatalogId)
                    .HasConstraintName("FK_Properties_Catalogs");
            });

            modelBuilder.Entity<View>(entity =>
            {
                entity.ToTable("View");

                entity.Property(e => e.ViewId).HasColumnName("ViewID");

                entity.Property(e => e.EventDate)
                    .HasColumnType("datetime")
                    .HasComputedColumnSql("(getdate())", false);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.UserId)
                    .HasMaxLength(450)
                    .HasColumnName("UserID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Views)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Views_Products");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
