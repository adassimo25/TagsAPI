using TagsAPI.Contracts.Dtos;
using TagsAPI.Contracts.Enums;

namespace TagsAPI.Services.Interfaces
{
    public interface ITagsService : IService
    {
        Task<IEnumerable<TagDto>> GetTags(int page, int limit, TagsSort sort, Order order);
        Task<SynchronizationResultDto> Synchronize();
    }
}
