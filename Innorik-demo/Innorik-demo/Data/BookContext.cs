using InnorikDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace InnorikDemo.Data
{
    public class InnorikDbContext : DbContext
    {
        public InnorikDbContext(DbContextOptions<InnorikDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Book>().ToTable("Books");
        }
    }
}
