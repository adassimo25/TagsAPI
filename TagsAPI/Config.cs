namespace TagsAPI
{
    public static class Config
    {
        public static class Database
        {
            public static string ConnectionString(IConfiguration cfg) => cfg.GetConnectionString("Database")!;
        }

        public static class Sections
        {
            public abstract class Section
            {
            }

            public class StackOverflowTags : Section
            {
                public string Url { get; set; } = null!;
                public int Minimal {  get; set; }
                public int PageSize { get; set; }
            }
        }
    }
}
