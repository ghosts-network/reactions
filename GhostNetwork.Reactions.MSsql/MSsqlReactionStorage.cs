using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GhostNetwork.Reactions.Domain;

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
            if (await context.ReactionEntities.AnyAsync(x => x.Key == key && x.Author == author))
            {
                return;
            }

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
            var result = await context.ReactionEntities
                .Where(x => x.Key == key)
                .ToDictionaryAsync(x => x.Id.ToString(), x => x.Type);

            return result
                .GroupBy(r => r.Value)
                .ToDictionary(rg => rg.Key, rg => rg.Count());
        }

        public async Task UpdateAsync(string key, string type, string author)
        {
            var entity = await context.ReactionEntities
                .FirstOrDefaultAsync(x => x.Key == key && x.Author == author);

            entity.Type = type;

            context.ReactionEntities.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string key, string author)
        {
            var entity = await context.ReactionEntities
                .SingleOrDefaultAsync(x => x.Key == key && x.Author == author);

            context.ReactionEntities.Remove(entity);

            await context.SaveChangesAsync();
        }
    }
}
