using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GhostNetwork.Reactions.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{entity}/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetAsync([FromRoute] string entity, [FromRoute] string id)
        {
            if (_reactionStorage.GetStats(entity, id) == null)
            {
                return NotFound();
            }

            return Ok(_reactionStorage.GetStats(entity, id));
        }

        [HttpPost("{entity}/{id}/{type}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddAsync([FromRoute] string entity, [FromRoute] string id,
            [FromHeader] string author, [FromRoute] string type)
        {
            _reactionStorage.AddAsync(entity, id, new ReactionEntity(author, type));

            return Ok(_reactionStorage.GetStats(entity, id));
        }
    }
}