using Microsoft.EntityFrameworkCore;


namespace GhostNetwork.Reactions.MSsql
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
    }
}
