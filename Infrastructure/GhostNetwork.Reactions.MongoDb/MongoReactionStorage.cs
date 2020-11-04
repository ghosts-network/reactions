using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace GhostNetwork.Reactions.MongoDb
{
    public class MongoReactionStorage : IReactionStorage
    {
        private readonly MongoDbContext context;

        public MongoReactionStorage(MongoDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(string key, string author, string type)
        {
            var entity = new ReactionEntity
            {
                Key = key,
                Author = author,
                Type = type
            };

            await context.Reactions.InsertOneAsync(entity);
        }

        public async Task<IDictionary<string, int>> GetStats(string key)
        {
            var filter = Builders<ReactionEntity>.Filter.Eq(p => p.Key, key);

            var result = await context.Reactions
                .Find(filter)
                .ToListAsync();

            return result
                .ToDictionary(x => x.Id.ToString(), x => x.Type)
                .GroupBy(r => r.Value)
                .ToDictionary(rg => rg.Key, rg => rg.Count());
        }

        public async Task UpdateAsync(string key, string type, string author)
        {
            var filter = Builders<ReactionEntity>.Filter.Eq(p => p.Key, key)
                         & Builders<ReactionEntity>.Filter.Eq(p => p.Author, author);

            var update = Builders<ReactionEntity>.Update
                .Set(r => r.Type, type);

            await context.Reactions.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(string key, string author)
        {
            var filter = Builders<ReactionEntity>.Filter.Eq(p => p.Key, key)
                         & Builders<ReactionEntity>.Filter.Eq(p => p.Author, author);

            await context.Reactions.DeleteOneAsync(filter);
        }
    }
}
