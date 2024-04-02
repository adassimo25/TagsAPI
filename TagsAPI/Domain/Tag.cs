namespace TagsAPI.Domain
{
    public class Tag
    {
        public Guid Id { get; private init; }
        public string Name { get; private init; } = null!;
        public int Count { get; private set; }
        public double Share { get; private set; }
        public DateTime? CreationDate { get; private init; } = null;
        public DateTime? ModificationDate { get; private set; } = null;

        public static Tag Create(Guid id, string name, DateTime creationDate)
        {
            return new()
            {
                Id = id,
                Name = name,
                CreationDate = creationDate,
                ModificationDate = creationDate,
            };
        }

        public void ChangeCount(int count, DateTime modificationDate)
        {
            Count = count;
            ModificationDate = modificationDate;
        }

        public void ChangeShare(double share, DateTime modificationDate)
        {
            Share = share;
            ModificationDate = modificationDate;
        }
    }
}
