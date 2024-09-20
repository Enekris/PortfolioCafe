using Cafe.Dal.Contracts.Repositories.Bot.Models;
using Cafe.Dal.Infrastructure.DbMaps;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Dal.Infrastructure.DBSettingsEF
{
    internal class BotTgContext : DbContext
    {
        public BotTgContext()
        {
        }

        public BotTgContext(DbContextOptions<BotTgContext> options)
            : base(options)
        {
           // Database.EnsureCreated();
        }

        public virtual DbSet<UserDataTgDb> UsersDataTg { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server=localhost;Database=BotTG;Trusted_Connection=True;TrustServerCertificate=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BotTgMap());

        }
    }

}
