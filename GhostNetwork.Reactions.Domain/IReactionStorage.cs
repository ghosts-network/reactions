using System.Collections.Generic;


namespace GhostNetwork.Reactions.Domain
{
    public interface IReactionStorage
    {
        IDictionary<string, int> GetStats(string entity, string id);

        void AddAsync(string entity, string id, Reaction reaction);

    }
}
