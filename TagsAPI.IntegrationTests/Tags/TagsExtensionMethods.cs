using TagsAPI.Contracts.Dtos;

namespace TagsAPI.IntegrationTests.Tags
{
    public static class TagsExtensionMethods
    {
        public static async Task GetTagsAsync(this HttpClient client, TestGetTagsRequest request, List<TestTag> tags)
        {
            var result = (await client.HandleGetRequestAsync<IEnumerable<TagDto>>(
                $"{ApiRoutes.Tags.GetTags}?page={request.Page}&pageSize={request.PageSize}&sort={request.Sort}&order={request.Order}"))
                .ToList();

            Assert.Equal(tags.Count, result.Count);

            for (var i = 0; i < tags.Count; i++)
            {
                Assert.Equal(tags[i].Name, result[i].Name);
                Assert.Equal(tags[i].Count, result[i].Count);
                Assert.True(Math.Abs(tags[i].Share - result[i].Share) <= Math.Pow(10, -6));
            }
        }

        public static async Task GetTagsAsync(this HttpClient client, TestGetTagsRequest request, params string[] errorCodes)
        {
            await client.HandleGetRequestAndEnsurePossibleErrorsAsync<IEnumerable<TagDto>>(
                $"{ApiRoutes.Tags.GetTags}?page={request.Page}&pageSize={request.PageSize}&sort={request.Sort}&order={request.Order}",
                errorCodes);
        }
    }
}