namespace TagsAPI.Contracts.Common
{
    public class PaginatedResult<T>
    {
        public List<T> Elements { get; set; } = null!;
        public int TotalElements { get; set; }
    }
}
