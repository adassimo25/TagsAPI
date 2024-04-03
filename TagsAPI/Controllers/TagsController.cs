using Microsoft.AspNetCore.Mvc;
using TagsAPI.Contracts.Dtos;
using TagsAPI.Contracts.Enums;
using TagsAPI.Contracts.Requests;
using TagsAPI.Services.Interfaces;

namespace TagsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController(IServiceProvider serviceProvider, ITagsService tagsService) : BaseTagsAPIController(serviceProvider)
    {
        private readonly ITagsService tagsService = tagsService;

        /// <summary>
        /// Get list of tags
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Number of elements on page</param>
        /// <param name="sort">Property the list is sorted by</param>
        /// <param name="order">Order of sorting</param>
        /// <returns>List of Tags sorted and ordered as requested</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetTags(int page, int pageSize, TagsSort sort = TagsSort.Count, Order order = Order.Desc)
        {
            var request = new GetTagsRequest()
            {
                Page = page,
                PageSize = pageSize,
                Sort = sort,
                Order = order
            };
            var vResult = ValidateRequest<GetTagsRequest>(request);

            return vResult ?? Ok(await tagsService.GetTags(page, pageSize, sort, order));
        }

        /// <summary>
        /// Synchronizes database with tags from StackOverflow API
        /// </summary>
        /// <returns>Synchronization summary</returns>
        [HttpPost("Synchronize")]
        public async Task<ActionResult<SynchronizationResultDto>> Synchronize()
        {
            return Ok(await tagsService.Synchronize());
        }
    }
}
