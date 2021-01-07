using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        /// Returns stats for one entity.
        /// </summary>
        /// <param name="key">Entity key</param>
        /// <response code="200">Returns stats for one entity by key.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{key}")]
        public async Task<ActionResult<IDictionary<string, int>>> GetAsync([FromRoute] string key)
        {
            var result = await reactionStorage.GetStats(key);

            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Returns reaction by author.
        /// </summary>
        /// <param name="key">Entity key</param>
        /// <param name="author">Author of reaction</param>
        /// <response code="200">Returns reaction by author and key.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{key}/author")]
        public async Task<ActionResult<Reaction>> GetReactionByAuthor(
            [FromRoute] string key,
            [Required, FromHeader] string author)
        {
            var result = await reactionStorage.GetReactionByAuthor(key, author);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Add type of reaction to entity.
        /// </summary>
        /// <param name="key">Entity key</param>
        /// <param name="type">Reaction type</param>
        /// <param name="author">Author of reaction</param>
        /// <response code="201">Reaction is added.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("{key}/{type}")]
        public async Task<ActionResult<IDictionary<string, int>>> UpsertAsync(
            [FromRoute] string key,
            [FromRoute] string type,
            [Required, FromHeader] string author)
        {
            await reactionStorage.UpsertAsync(key, author, type);

            return Ok(await reactionStorage.GetStats(key));
        }

        /// <summary>
        /// Remove type of reaction.
        /// </summary>
        /// <param name="key">Entity key</param>
        /// <param name="author">Author of reaction</param>
        /// <response code="200">Remove reaction by key and author.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{key}")]
        public async Task<ActionResult<IDictionary<string, int>>> DeleteAsync(
            [FromRoute] string key,
            [Required, FromHeader] string author)
        {
            await reactionStorage.DeleteAsync(key, author);

            return Ok(await reactionStorage.GetStats(key));
        }
    }
}