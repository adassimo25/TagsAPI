using Microsoft.AspNetCore.Mvc;
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
    }
}
