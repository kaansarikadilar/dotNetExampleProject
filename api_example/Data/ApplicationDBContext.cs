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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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