using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TagsAPI.Contracts.Dtos;
using TagsAPI.Contracts.Enums;
using TagsAPI.DataAccess;
using TagsAPI.DataAccess.Repositories;
using TagsAPI.Domain;
using TagsAPI.Services.Interfaces;

namespace TagsAPI.Services
{
    public class TagsService(IRepository<Tag, Guid> tagsRepository, TagsDbContext dbContext,
        IStackOverflowAccessService stackOverflowAccessService, ILogger<TagsService> logger) : ITagsService
    {
        private readonly IRepository<Tag, Guid> tagsRepository = tagsRepository;
        private readonly TagsDbContext dbContext = dbContext;
        private readonly IStackOverflowAccessService stackOverflowAccessService = stackOverflowAccessService;
        private readonly ILogger<TagsService> logger = logger;

        public Task<IEnumerable<TagDto>> GetTags(int page, int limit, TagsOrder order)
        {
            throw new NotImplementedException();
        }

        public async Task<SynchronizationResultDto> Synchronize()
        {
            var tagsFromAPI = await stackOverflowAccessService.GetTags();
            var tagsFromDb = await dbContext.Tags.ToListAsync();

            var totalCount = tagsFromAPI.Sum(x => x.Count);

            var tagsToAdd = new List<Tag>();
            var tagsToUpdate = new List<Tag>();

            foreach (var tagFromAPI in tagsFromAPI)
            {
                var share = (double)tagFromAPI.Count / totalCount * 100;

                var existingTag = tagsFromDb.FirstOrDefault(t => t.Name == tagFromAPI.Name);
                if (existingTag == null)
                {
                    var newTag = Tag.Create(Guid.NewGuid(),
                        tagFromAPI.Name,
                        tagFromAPI.Count,
                        share,
                        DateTime.Now);
                    tagsToAdd.Add(newTag);
                }
                else
                {
                    if (existingTag.UpdateIfNeeded(tagFromAPI.Count, share, DateTime.Now))
                    {
                        tagsToUpdate.Add(existingTag);
                    }

                    tagsFromDb.Remove(existingTag);
                }
            }

            await tagsRepository.AddRangeAsync(tagsToAdd);
            await tagsRepository.UpdateRangeAsync(tagsToUpdate);
            await tagsRepository.RemoveRangeAsync(tagsFromDb);

            var result = new SynchronizationResultDto()
            {
                Created = tagsToAdd.Count,
                Updated = tagsToUpdate.Count,
                Deleted = tagsToUpdate.Count
            };

            logger.LogInformation("Synchronization result: {SynchronizationResult}", JsonConvert.SerializeObject(result));

            return result;
        }
    }
}
