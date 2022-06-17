using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RozetkaWebApp.Models;

#nullable disable

namespace RozetkaWebApp.Data
{
    public partial class RozetkadbContext : DbContext
    {
        public RozetkadbContext()
        {
        }

        public RozetkadbContext(DbContextOptions<RozetkadbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
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
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Filter> Filters { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<LineDetail> LineDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Portal> Portals { get; set; }
        public virtual DbSet<PortalImage> PortalImages { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<Promotion> Promotions { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<RootImage> RootImages { get; set; }
        public virtual DbSet<View> Views { get; set; }
        public virtual DbSet<Wallett> Walletts { get; set; }

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

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.AddressLine1)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.AddressLine2).HasMaxLength(100);

                entity.Property(e => e.AddressLine3).HasMaxLength(100);

                entity.Property(e => e.AddressType).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.ExtAddressId).HasColumnName("ExtAddressID");

                entity.Property(e => e.PostalCode).HasMaxLength(50);

                entity.Property(e => e.State).HasMaxLength(50);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Address_AspNetUsers");
            });

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

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.CatalogImages)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatalogImage_Image1");
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

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.Cons).HasMaxLength(500);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Pros).HasMaxLength(500);

                entity.Property(e => e.Score).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Comment_Image");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Comment_Product");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Comment_AspNetUsers");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.AssignDate).HasColumnType("date");

                entity.Property(e => e.Attention).HasMaxLength(150);

                entity.Property(e => e.ContactType).HasMaxLength(100);

                entity.Property(e => e.DefAddressId).HasColumnName("DefAddressID");

                entity.Property(e => e.DisplayName).HasMaxLength(150);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.ExtAddressId).HasColumnName("ExtAddressID");

                entity.Property(e => e.Fax).HasMaxLength(150);

                entity.Property(e => e.FaxType).HasMaxLength(150);

                entity.Property(e => e.FirstName).HasMaxLength(150);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.LastName).HasMaxLength(150);

                entity.Property(e => e.MidName).HasMaxLength(150);

                entity.Property(e => e.Phone1).HasMaxLength(50);

                entity.Property(e => e.Phone1Type).HasMaxLength(150);

                entity.Property(e => e.Phone2).HasMaxLength(50);

                entity.Property(e => e.Phone2Type).HasMaxLength(150);

                entity.Property(e => e.Phone3).HasMaxLength(50);

                entity.Property(e => e.Phone3Type).HasMaxLength(150);

                entity.Property(e => e.Salutation).HasMaxLength(150);

                entity.Property(e => e.Title).HasMaxLength(150);

                entity.Property(e => e.UserId)
                    .HasMaxLength(450)
                    .HasColumnName("UserID");

                entity.Property(e => e.WebSite).HasMaxLength(150);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Contact_AspNetUsers");
            });

            modelBuilder.Entity<Filter>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Filters");

                entity.Property(e => e.CatalogId).HasColumnName("CatalogID");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PropertyId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<LineDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDatailId)
                    .HasName("PK_OrderDetail");

                entity.ToTable("LineDetail");

                entity.Property(e => e.CartId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExtOrderDetailNbr).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.UnitCost).HasColumnType("money");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.LineDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_OrderDetail_Order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.LineDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_OrderDetail_Product");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.CardNumber).HasMaxLength(50);

                entity.Property(e => e.DeliveryAddress)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.DeliveryContact)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.DeliveryEmail).HasMaxLength(50);

                entity.Property(e => e.DeliveryPhone).HasMaxLength(50);

                entity.Property(e => e.ExtOrderNbr).HasMaxLength(50);

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.Total).HasColumnType("money");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Order_AspNetUsers");
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

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PortalId).HasColumnName("PortalID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.PortalImages)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PortalImage_Image");

                entity.HasOne(d => d.Portal)
                    .WithMany(p => p.PortalImages)
                    .HasForeignKey(d => d.PortalId)
                    .HasConstraintName("FK_PortalImage_Portal");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CatalogId).HasColumnName("CatalogID");

                entity.Property(e => e.ChioseData).HasColumnType("datetime");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.Quantity).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ViewDate).HasColumnType("datetime");

                entity.HasOne(d => d.Catalog)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CatalogId)
                    .HasConstraintName("FK_Products_Catalogs");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.PromotionId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Product_Promotion");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.ToTable("ProductImage");

                entity.Property(e => e.ProductImageId).HasColumnName("ProductImageID");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_ProductImage_Image");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductImages_Products");
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.ToTable("Promotion");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Promotions)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Promotion_Image");
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

            modelBuilder.Entity<RootImage>(entity =>
            {
                entity.ToTable("RootImage");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.RootImages)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_RootImage_Image");
            });

            modelBuilder.Entity<View>(entity =>
            {
                entity.ToTable("View");

                entity.Property(e => e.ViewId).HasColumnName("ViewID");

                entity.Property(e => e.CartId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.EventDate).HasColumnType("datetime");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.UserId)
                    .HasMaxLength(450)
                    .HasColumnName("UserID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Views)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Views_Products");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Views)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_View_AspNetUsers");
            });

            modelBuilder.Entity<Wallett>(entity =>
            {
                entity.HasKey(e => e.WalletId)
                    .HasName("PK_WalletId");

                entity.ToTable("Wallett");

                entity.Property(e => e.CardNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CardType).HasMaxLength(50);

                entity.Property(e => e.Cardholder)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ExpiryDate)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.VerificationCode).HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Walletts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_WalletId_AspNetUsers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
