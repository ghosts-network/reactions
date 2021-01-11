﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Reactions
{
    public interface IReactionStorage
    {
        Task<IDictionary<string, int>> GetStats(string key);

        Task<Reaction> GetReactionByAuthor(string key, string author);

        Task<IDictionary<string, Dictionary<string, int>>> GetReactionsForManyPublications(string[] keys);

        Task UpsertAsync(string key, string author, string type);

        Task DeleteByAuthorAsync(string key, string author);

        Task DeleteAsync(string key);
    }
}
