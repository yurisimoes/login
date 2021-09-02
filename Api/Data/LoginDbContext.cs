using System.Reflection;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class LoginDbContext : DbContext
    {
        public LoginDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}