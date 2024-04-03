using Microsoft.AspNetCore.Mvc;
using TagsAPI.Contracts.Dtos;
using TagsAPI.Services.Interfaces;

namespace TagsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController(IServiceProvider serviceProvider, ITagsService tagsService) : BaseTagsAPIController(serviceProvider)
    {
        private readonly ITagsService tagsService = tagsService;

        /// <summary>
        /// TODO
        /// </summary>
        [HttpGet]
        public ActionResult Todo()
        {
            return Ok();
        }

        /// <summary>
        /// Synchronizes database with tags from StackOverflow API
        /// </summary>
        [HttpPost("Synchronize")]
        public async Task<ActionResult<SynchronizationResultDto>> Synchronize()
        {
            return Ok(await tagsService.Synchronize());
        }
    }
}
