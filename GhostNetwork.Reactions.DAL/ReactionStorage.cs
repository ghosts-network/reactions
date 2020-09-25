using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostNetwork.Reactions.DAL
{
    public class ReactionStorage : IReactionStorage
    {
        private static IDictionary<string, IDictionary<string, List<ReactionEntity>>> storage = new Dictionary<string, IDictionary<string, List<ReactionEntity>>>();

        public void AddAsync(string entity, string id, ReactionEntity reaction)
        {
            if (!storage.ContainsKey(entity))
            {
                storage[entity] = new Dictionary<string, List<ReactionEntity>>();
            }


            if (!storage[entity].ContainsKey(id))
            {
                storage[entity][id] = new List<ReactionEntity>();
            }

            storage[entity][id].Add(reaction);
        }

        public IDictionary<string, int> GetStats(string entity, string id)
        {
            if (!storage.ContainsKey(entity) || !storage[entity].ContainsKey(id))
            {
                return new Dictionary<string, int>();
            }

            return storage[entity][id].GroupBy(r => r.Type)
                .ToDictionary(rg => rg.Key, rg => rg.Count());
        }


    }
}
