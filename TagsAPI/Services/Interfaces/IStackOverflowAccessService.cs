using TagsAPI.Contracts.Dtos;

namespace TagsAPI.Services.Interfaces
{
    public interface IStackOverflowAccessService : IService
    {
        Task<IEnumerable<TagFromAPIDto>> GetTags();
    }
}
