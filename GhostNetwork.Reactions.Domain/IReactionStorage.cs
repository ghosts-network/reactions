using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Reactions
{
    public interface IReactionStorage
    {
        Task<IDictionary<string, int>> GetStats(string key);

        Task AddAsync(string key, string author, string type);

        Task DeleteAsync(string key, string author);

        Task UpdateAsync(string key, string type, string author);
    }
}
