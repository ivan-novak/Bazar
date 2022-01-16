﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using RozetkaWebApp.Models;

namespace RozetkaWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<RozetkaWebApp.Models.Portal> Portal { get; set; }
        public DbSet<RozetkaWebApp.Models.Catalog> Catalog { get; set; }
        public DbSet<RozetkaWebApp.Models.PortalImage> PortalImage { get; set; }
        public DbSet<RozetkaWebApp.Models.ControlImage> ControlImage { get; set; }
        public DbSet<RozetkaWebApp.Models.Product> Product { get; set; }
    }
}