using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_example.Models;
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
    }
}