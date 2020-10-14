using Microsoft.EntityFrameworkCore;


namespace GhostNetwork.Reactions.Mssql
{
    public class MssqlContext : DbContext
    {
        public MssqlContext(DbContextOptions<MssqlContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<ReactionEntity> ReactionEntities { get; set; }
    }
}
