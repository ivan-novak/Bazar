using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using RozetkaWebApp.Models;
using System.ComponentModel;

namespace RozetkaWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        [DisplayName("Портал")]
        public DbSet<RozetkaWebApp.Models.Portal> Portal { get; set; }
        [DisplayName("Каталог")]
        public DbSet<RozetkaWebApp.Models.Catalog> Catalog { get; set; }
        [DisplayName("Изображения портала")]
        public DbSet<RozetkaWebApp.Models.PortalImage> PortalImage { get; set; }
        public DbSet<RozetkaWebApp.Models.ControlImage> ControlImage { get; set; }
        public DbSet<RozetkaWebApp.Models.Product> Product { get; set; }
        public DbSet<RozetkaWebApp.Models.Property> Property { get; set; }
        public DbSet<RozetkaWebApp.Models.Characteristic> Characteristic { get; set; }
        public DbSet<RozetkaWebApp.Models.CatalogImage> CatalogImage { get; set; }
        public DbSet<RozetkaWebApp.Models.Image> Image { get; set; }
        public DbSet<RozetkaWebApp.Models.ProductImage> ProductImage { get; set; }
    }
}
