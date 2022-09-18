namespace Trucks.Data
{
    using LeagueChampsDb.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class LeagueChampsContext : DbContext
    {
        public DbSet<Champion> Champions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserChampion> UsersChampions { get; set; }

        public LeagueChampsContext() { }

        public LeagueChampsContext(DbContextOptions options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserChampion>(e=>e.HasKey(k=> new { k.UserId, k.ChampionId }));
        }
    }
}
