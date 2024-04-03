using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;
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

        public async Task<IEnumerable<TagDto>> GetTags(int page, int pageSize, TagsSort sort, Order order)
        {
            var tagsPaginatedQuery = new TagsPaginatedQuery
            {
                PageNumber = page,
                PageSize = pageSize
            };

            Expression<Func<Tag, object>> sortBy = sort switch
            {
                TagsSort.Count => tag => tag.Count,
                TagsSort.Share => tag => tag.Share,
                _ => throw new ArgumentException($"Invalid sort option: {sort}"),
            };

            var tagsPaginated = await dbContext.Tags
                .AsNoTracking()
                .OrderBy(sortBy, order == Order.Desc)
                .Paginated(tagsPaginatedQuery)
                .Select(t => new TagDto()
                {
                    Name = t.Name,
                    Count = t.Count,
                    Share = (float)t.Share,
                    ModificationDate = t.ModificationDate
                })
                .ToListAsync();

            return tagsPaginated;
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
                Deleted = tagsFromDb.Count
            };

            logger.LogInformation("Synchronization result: {SynchronizationResult}", JsonConvert.SerializeObject(result));

            return result;
        }
    }
}
