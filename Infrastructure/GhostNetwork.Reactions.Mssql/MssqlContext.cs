using Microsoft.EntityFrameworkCore;

namespace GhostNetwork.Reactions.Mssql
{
    public class MssqlContext : DbContext
    {
        public MssqlContext(DbContextOptions<MssqlContext> options)
            : base(options)
        {
        }

        public DbSet<ReactionEntity> ReactionEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
