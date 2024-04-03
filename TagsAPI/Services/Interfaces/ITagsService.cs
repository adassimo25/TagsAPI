using TagsAPI.Contracts.Dtos;
using TagsAPI.Contracts.Enums;

namespace TagsAPI.Services.Interfaces
{
    public interface ITagsService : IService
    {
        Task<IEnumerable<TagDto>> GetTags(int page, int limit, TagsOrder order);
        Task<SynchronizationResultDto> Synchronize();
    }
}
