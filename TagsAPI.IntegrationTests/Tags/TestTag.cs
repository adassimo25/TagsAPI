using TagsAPI.Contracts.Enums;

namespace TagsAPI.IntegrationTests.Tags
{
    public record TestTag
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Tag";
        public int Count { get; set; } = 10;
        public double Share { get; set; } = 0.5;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime ModificationDate { get; set; } = DateTime.Now;
    }

    public record TestGetTagsRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public TagsSort Sort { get; set; }
        public Order Order { get; set; }
    }
}
