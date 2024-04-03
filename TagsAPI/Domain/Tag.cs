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

        public static Tag Create(Guid id, string name, int count, double share, DateTime creationDate)
        {
            return new()
            {
                Id = id,
                Name = name,
                Count = count,
                Share = share,
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

        public bool UpdateIfNeeded(int count, double share, DateTime updateTime)
        {
            var updated = false;

            if (Count != count)
            {
                ChangeCount(count, updateTime);
                updated = true;
            }

            if (Math.Abs(Share - share) >= Math.Pow(10, -6))
            {
                ChangeShare(share, updateTime);
                updated = true;
            }

            return updated;
        }
    }
}
