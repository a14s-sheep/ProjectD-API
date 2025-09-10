using Microsoft.EntityFrameworkCore;
using ProjectD_API.Data.Models;

namespace ProjectD_API.Data
{
    public class GameDBContext : DbContext
    {
        public GameDBContext(DbContextOptions<GameDBContext> options) : base(options) { }

        // GameServerInfo
        public DbSet<GameServerInfo> GameServerInfos { get; set; }

        // Users
        public DbSet<User> Users { get; set; }

        // Players
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerItem> PlayerItems { get; set; }
        public DbSet<PlayerQuest> PlayerQuests { get; set; }
        public DbSet<PlayerTask> PlayerTasks { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Users
            builder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            builder.Entity<User>().Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<User>().Property(u => u.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

            // Players
            builder.Entity<Player>().HasIndex(p => p.Name).IsUnique();
            builder.Entity<Player>().Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<Player>().Property(u => u.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

            // PlayerItems
            builder.Entity<PlayerItem>().Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<PlayerItem>().Property(u => u.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

            // PlayerQuests
            builder.Entity<PlayerQuest>().Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<PlayerQuest>().Property(u => u.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

            // PlayerTasks
            builder.Entity<PlayerTask>().Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<PlayerTask>().Property(u => u.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is User user)
                {
                    user.UpdatedAt = DateTime.UtcNow;
                }
                if (entry.Entity is Player player)
                {
                    player.UpdatedAt = DateTime.UtcNow;
                }
            }
            return base.SaveChanges();
        }
    }
}
