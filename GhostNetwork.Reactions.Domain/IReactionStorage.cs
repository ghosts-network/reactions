using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GhostNetwork.Reactions.Domain
{
    public interface IReactionStorage
    {
        Task<IDictionary<string, int>> GetStats(string key);

        Task AddAsync(string key, string author, string type);

        Task DeletAsync(string key);

        Task UpdateAsync(string key, string type, string author);

        Task<bool> AuthorReaction(string key, string author);
    }
}
