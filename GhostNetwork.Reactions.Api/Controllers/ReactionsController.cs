using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GhostNetwork.Reactions.MSsql;
using GhostNetwork.Reactions.Domain;

namespace GhostNetwork.Reactions.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReactionsController : ControllerBase
    {
        private readonly IReactionStorage _reactionStorage;

        public ReactionsController(IReactionStorage storage)
        {
            _reactionStorage = storage;
        }

        /// <summary>
        /// Returns stats for one entity
        /// </summary>
        /// <response code="200">Returns stats for one entity</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{entity}/{id}")]
        public ActionResult<IDictionary<string, int>> GetAsync([FromRoute] string entity, [FromRoute] string id)
        {
            if (_reactionStorage.GetStats(entity, id) == null)
            {
                return NotFound();
            }

            return Ok(_reactionStorage.GetStats(entity, id));
        }

        /// <summary>
        /// Add type of reaction to entity
        /// </summary>
        /// <response code="201">Add type of reaction to entity</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("{entity}/{id}/{type}")]
        public ActionResult<Dictionary<string, List<ReactionEntity>>> AddAsync([FromRoute] string entity, [FromRoute] string id,
            [FromHeader] string author, [FromRoute] string type)
        {
            _reactionStorage.AddAsync(entity, id, new Reaction(author, type));

            return Ok(_reactionStorage.GetStats(entity, id));
        }
    }
}