using ProjectD_API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProjectD_API.Data
{
    public class GameDBContext : IdentityDbContext<User>
    {
        public GameDBContext(DbContextOptions<GameDBContext> options) : base(options) { }

        // GameServerInfo
        public DbSet<GameServerInfo> GameServerInfos { get; set; }

        // GameDefaultValues
        public DbSet<ClassDefaultStat> ClassDefaultStats { get; set; }
        public DbSet<ClassDefaultItem> ClassDefaultItems { get; set; }

        // Token
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // ResetPasswordPin
        public DbSet<PasswordResetRecord> PasswordResetRecords { get; set; }

        // Users
        public DbSet<User> Users { get; set; }

        // Players
        public DbSet<Player> Players { get; set; }
        public DbSet<CharacterClass> CharacterClasses { get; set; }
        public DbSet<PlayerItem> PlayerItems { get; set; }
        public DbSet<PlayerQuest> PlayerQuests { get; set; }
        public DbSet<PlayerTask> PlayerTasks { get; set; }
        public DbSet<PlayerSkill> PlayerSkills { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Tokens
            builder.Entity<RefreshToken>().Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Users
            builder.Entity<User>(e => e.ToTable("users"));
            builder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
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
