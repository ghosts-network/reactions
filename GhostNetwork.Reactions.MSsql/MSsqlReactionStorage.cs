using GhostNetwork.Reactions.Domain;
using System.Collections.Generic;
using System.Linq;


namespace GhostNetwork.Reactions.MSsql
{
    public class MSsqlReactionStorage : IReactionStorage
    {
        private static IDictionary<string, IDictionary<string, List<Reaction>>> storage = new Dictionary<string, IDictionary<string, List<Reaction>>>();

        public void AddAsync(string entity, string id, Reaction reaction)
        {
            if (!storage.ContainsKey(entity))
            {
                storage[entity] = new Dictionary<string, List<Reaction>>();
            }


            if (!storage[entity].ContainsKey(id))
            {
                storage[entity][id] = new List<Reaction>();
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
