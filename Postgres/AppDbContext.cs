using Microsoft.EntityFrameworkCore;
using Postgres.Models;


namespace Postgres
{
    public class AppDbContext:DbContext
    {
        // Контекс бд
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Таблички бд
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderFile> OrderFiles { get; set; }

        // наработка метода для миграций в бд
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<OrderFile>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.UploadedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }

    }

}
