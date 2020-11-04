using MongoDB.Driver;

namespace GhostNetwork.Reactions.MongoDb
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase database;

        public MongoDbContext(IMongoDatabase database)
        {
            this.database = database;
        }

        public IMongoCollection<ReactionEntity> Reactions =>
            database.GetCollection<ReactionEntity>("reactions");
    }
}
