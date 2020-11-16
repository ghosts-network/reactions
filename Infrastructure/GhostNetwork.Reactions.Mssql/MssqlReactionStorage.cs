using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GhostNetwork.Reactions.Mssql
{
    public class MssqlReactionStorage : IReactionStorage
    {
        private readonly MssqlContext context;

        public MssqlReactionStorage(MssqlContext context)
        {
            this.context = context;
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

        public async Task UpsertAsync(string key, string author, string type)
        {
            var existing = await context.ReactionEntities.FirstOrDefaultAsync(r => r.Key == key && r.Author == author);

            if (existing != null)
            {
                existing.Type = type;

                context.ReactionEntities.Update(existing);
                await context.SaveChangesAsync();

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

        public async Task DeleteAsync(string key, string author)
        {
            var entity = await context.ReactionEntities
                .SingleOrDefaultAsync(x => x.Key == key && x.Author == author);

            context.ReactionEntities.Remove(entity);

            await context.SaveChangesAsync();
        }
    }
}
