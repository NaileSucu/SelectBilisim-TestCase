using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class Context : IdentityDbContext<User,Role,int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=NAILE\\SQLEXPRESS;Database=SelectBilisimCase;User Id=sa;Password=1;TrustServerCertificate=True;");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLogins> UserLogin { get; set; }
    }
}
