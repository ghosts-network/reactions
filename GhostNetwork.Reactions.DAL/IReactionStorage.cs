using System;
using System.Collections.Generic;
using System.Text;

namespace GhostNetwork.Reactions.DAL
{
    public interface IReactionStorage
    {
        IDictionary<string, int> GetStats(string entity, string id);

        public void AddAsync(string entity, string id, ReactionEntity reaction);
        
    }
}
