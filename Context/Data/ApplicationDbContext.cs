using System;
using System.Collections.Generic;
using System.Text;
using Context.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Context.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            ApplicationUser user = new ApplicationUser
            {
                FirstName = "Alejandro",
                LastName = "Font",
                UserName = "Alejandro10",
                NormalizedUserName = "Alejandro",
                Email = "al@me.com",
                NormalizedEmail = "al@me.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            var passwordHash = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHash.HashPassword(user, "Alex*10");
            modelBuilder.Entity<ApplicationUser>().HasData(user);


            modelBuilder.Entity<Book>().HasData(
                          new Book()
                          {
                              BookId = 1,
                              Title = "Crime and Punishment",
                              AuthorFirstName = "Fyodor",
                              AuthorLastName = "Dostoevsky",
                              Country = "Russia",
                              HistoricalLink = "https://en.wikipedia.org/wiki/Emancipation_reform_of_1861"

                          });

        }
    }
}