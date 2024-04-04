namespace TagsAPI.IntegrationTests
{
    public static class ApiRoutes
    {
        public new static string ToString() => $"api";

        public static class Tags
        {
            public new static string ToString() => $"{ApiRoutes.ToString()}/tags";

            public static string GetTags => ToString();
        }
    }
}