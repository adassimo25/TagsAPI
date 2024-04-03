namespace TagsAPI.Contracts.Dtos
{
    public class TagsFromAPICollectionDto
    {
        public IEnumerable<TagFromAPIDto> Items { get; set; } = null!;
    }
}
