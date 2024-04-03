namespace TagsAPI.Contracts.Dtos
{
    public class TagDto
    {
        public string Name { get; set; } = null!;
        public int Count { get; set; }
        public double Share { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}
