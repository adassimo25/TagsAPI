namespace TagsAPI.Contracts.Common
{
    public abstract class PaginatedQuery<TResult>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
