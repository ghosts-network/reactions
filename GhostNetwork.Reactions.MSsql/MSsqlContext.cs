using Microsoft.EntityFrameworkCore;


namespace GhostNetwork.Reactions.MSsql
{
    public class MSsqlContext : DbContext
    {
        public MSsqlContext(DbContextOptions<MSsqlContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<ReactionEntity> ReactionEntities { get; set; }
    }
}
