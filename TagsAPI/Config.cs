namespace TagsAPI
{
    public static class Config
    {
        public static class Database
        {
            public static string ConnectionString(IConfiguration cfg) => cfg.GetConnectionString("Database")!;
        }
    }
}
