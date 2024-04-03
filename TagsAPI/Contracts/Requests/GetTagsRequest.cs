using TagsAPI.Contracts.Enums;

namespace TagsAPI.Contracts.Requests
{
    public class GetTagsRequest : PaginatedRequest
    {
        public TagsSort Sort { get; set; }
        public Order Order { get; set; }
    }
}
