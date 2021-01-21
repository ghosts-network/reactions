using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Reactions
{
    public interface IReactionStorage
    {
        Task<IDictionary<string, int>> GetStats(string key);

        Task<Reaction> GetReactionByAuthorAsync(string key, string author);

        Task<IEnumerable<Reaction>> GetReactionsByAuthorAsync(string author, string[] keys);

        Task<IDictionary<string, Dictionary<string, int>>> GetGroupedReactionsAsync(string[] keys);

        Task UpsertAsync(string key, string author, string type);

        Task DeleteByAuthorAsync(string key, string author);

        Task DeleteAsync(string key);
    }
}
