using Calzolari.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Calzolari.WebApi.Database
{
    public class DemoDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }

        public DemoDbContext(DbContextOptions<DemoDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasKey(c => c.CountryId);
        }
    }
}