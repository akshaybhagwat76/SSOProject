using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.SQLServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FieldType> FieldTypes { get; set; }
        public DbSet<ModuleDetails> ModuleDetails { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TenantClaims> TenantClaims { get; set; }
        public DbSet<TenantRoles> TenantRoles { get; set; }


        //ModuleFieldOption
        public DbSet<ModuleFieldDetails> ModuleFieldDetails { get; set; }
        public DbSet<ModuleFieldOptions> ModuleFieldOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<TenantRoles>()
            .HasKey(c => new { c.TenantID, c.RoleID });

        }
    }
}
