using TagsAPI.Contracts.Dtos;
using TagsAPI.Services.Interfaces;
using StackOverFlowTagsConfig = TagsAPI.Config.Sections.StackOverflowTags;

namespace TagsAPI.Services
{
    public class StackOverflowAccessService(IExternalAPIService<TagsFromAPICollectionDto> externalAPITagsCollectionService,
        StackOverFlowTagsConfig config) : IStackOverflowAccessService
    {
        private readonly IExternalAPIService<TagsFromAPICollectionDto> externalAPITagsCollectionService = externalAPITagsCollectionService;
        private readonly StackOverFlowTagsConfig config = config;

        public async Task<IEnumerable<TagFromAPIDto>> GetTags()
        {
            var tasks = new List<Task>();
            var tagsFromAPI = new List<TagFromAPIDto>();

            var pages = (int)Math.Ceiling((double)config.Minimal / config.PageSize);

            for (var page = 0; page < pages; page++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var pageTags = await GetTagsFromPage(page);

                    lock (tagsFromAPI)
                    {
                        tagsFromAPI.AddRange(pageTags.Items ?? new List<TagFromAPIDto>());
                    }
                }));
            }

            await Task.WhenAll(tasks);

            tagsFromAPI = tagsFromAPI.DistinctBy(x => x.Name).ToList();
            while (tagsFromAPI.Count < config.Minimal)
            {
                var pageTags = await GetTagsFromPage(++pages);

                tagsFromAPI = tagsFromAPI.UnionBy(pageTags.Items, x => x.Name).ToList();
            }

            return tagsFromAPI;
        }

        private async Task<TagsFromAPICollectionDto> GetTagsFromPage(int page)
        {
            var url = $"{config.Url}&page={page}&pagesize={config.PageSize}";
            var pageTags = await externalAPITagsCollectionService.GetResources(url);

            return pageTags;
        }
    }
}
