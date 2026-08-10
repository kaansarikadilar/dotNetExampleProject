using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_example.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api_example.Data
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>(x=> x.HasKey(p => new{p.AppUserId,p.StockId}));

            builder.Entity<Portfolio>()
                .HasOne(u=>u.AppUser)
                .WithMany(u=>u.Portfolios)
                .HasForeignKey(p=>p.AppUserId);

            builder.Entity<Portfolio>()
                .HasOne(u=>u.Stock)
                .WithMany(u=>u.Portfolios)
                .HasForeignKey(p=>p.StockId);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "ADMIN_ROLE",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "1"
                },
                 new IdentityRole
                {
                    Id = "USER_ROLE",
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "2"
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}