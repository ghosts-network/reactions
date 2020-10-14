using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IDictionary<string, int>>> AddAsync(
            [FromRoute] string key,
            [FromRoute] string type,
            [FromHeader] string author)
        {
            await reactionStorage.AddAsync(key, author, type);

            return Ok(await reactionStorage.GetStats(key));
        }

        /// <summary>
        /// Remove type of reaction
        /// </summary>
        /// <response code="200">Remove reaction by key and author</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{key}")]
        public async Task<ActionResult<IDictionary<string, int>>> DeleteAsync([FromRoute] string key, [FromHeader] string author)
        {
            await reactionStorage.DeleteAsync(key, author);

            return Ok(await reactionStorage.GetStats(key));
        }

        /// <summary>
        /// Update type of reaction
        /// </summary>
        /// <response code="200">Update type of reaction by key and author</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{key}/{type}")]
        public async Task<ActionResult<IDictionary<string, int>>> UpdateAsync(
            [FromRoute] string key,
            [FromRoute] string type,
            [FromHeader] string author)
        {
            await reactionStorage.UpdateAsync(key, type, author);

            return Ok(await reactionStorage.GetStats(key));
        }
    }
}