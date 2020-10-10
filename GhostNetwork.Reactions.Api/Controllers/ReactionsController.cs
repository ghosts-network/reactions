using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GhostNetwork.Reactions.Domain;
using System.Collections.Generic;

namespace GhostNetwork.Reactions.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReactionsController : ControllerBase
    {
        private readonly IReactionStorage reactionStorage;

        public ReactionsController(IReactionStorage reactionStorage)

        {
            this.reactionStorage = reactionStorage;
        }

        /// <summary>
        /// Returns stats for one entity
        /// </summary>
        /// <response code="200">Returns stats for one entity</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{key}")]
        public async Task<ActionResult<IDictionary<string, int>>> GetAsync([FromRoute] string key)
        {
            return Ok(await reactionStorage.GetStats(key));
        }

        /// <summary>
        /// Add type of reaction to entity
        /// </summary>
        /// <response code="201">Add type of reaction to entity</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("{key}/{type}")]
        public async Task<ActionResult<IDictionary<string, int>>> AddAsync([FromRoute] string key, 
            [FromRoute] string type, [FromHeader] string author)
        {
            await reactionStorage.AddAsync(key, author, type);

            return Ok(await reactionStorage.GetStats(key));
        }
    }
}