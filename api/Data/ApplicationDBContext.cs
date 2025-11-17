using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) 
            : base(dbContextOptions)
        {
            
        }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));

            List<IdentityRole> roles =
            [
                new() {
                    Id = "10c5f47b-25c1-4f32-8a92-0783696f71b2",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new() {
                    Id = "ea156066-981e-48a4-90e0-44ba7d47f0c5",
                    Name = "User",
                    NormalizedName = "USER"
                }
            ];

            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}