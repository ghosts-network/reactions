using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace GhostNetwork.Reactions.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReactionsController : ControllerBase
    {
        private static ReactionStorage storage = new ReactionStorage();

        [HttpGet("{entity}/{id}")]
        public ActionResult GetAsync([FromRoute] string entity, [FromRoute] string id)
        {
            return Ok(storage.GetStats(entity, id));
        }

        [HttpPost("{entity}/{id}/{type}")]
        public ActionResult AddAsync([FromRoute] string entity, [FromRoute] string id,
            [FromHeader] string author, [FromRoute] string type)
        {
            storage.AddAsync(entity, id, new Reaction(author, type));

            return Ok(storage.GetStats(entity, id));
        }
    }

    public class ReactionStorage
    {
        private static IDictionary<string, IDictionary<string, List<Reaction>>> storage = new Dictionary<string, IDictionary<string, List<Reaction>>>();

        public IDictionary<string, int> GetStats(string entity, string id)
        {
            if (!storage.ContainsKey(entity) || !storage[entity].ContainsKey(id))
            {
                return new Dictionary<string, int>();
            }

            return storage[entity][id].GroupBy(r => r.Type)
                .ToDictionary(rg => rg.Key, rg => rg.Count());
        }

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
    }

    public class Reaction
    {
        public Reaction(string author, string type)
        {
            Author = author;
            Type = type;
        }

        public string Author { get; }
        public string Type { get; }
    }
}