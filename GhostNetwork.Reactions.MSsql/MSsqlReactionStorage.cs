using GhostNetwork.Reactions.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GhostNetwork.Reactions.MSsql
{
    public class MSsqlReactionStorage : IReactionStorage
    {
        private readonly MSsqlContext context;

        public MSsqlReactionStorage(MSsqlContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(string key, string author, string type)
        {
            var created = new ReactionEntity
            {
                Id = Guid.NewGuid(),
                Key = key,
                Author = author,
                Type = type
            };

            await context.ReactionEntities.AddAsync(created);
            await context.SaveChangesAsync();
        }

        public async Task<IDictionary<string, int>> GetStats(string key)
        {
            var result = await context.ReactionEntities.Where(x => x.Key == key)
                .ToDictionaryAsync(x => x.Id.ToString(), x => x.Type);

            return result.GroupBy(r => r.Value).ToDictionary(rg => rg.Key, rg => rg.Count());
        }
    }
}
