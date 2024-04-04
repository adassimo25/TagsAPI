using Newtonsoft.Json;
using System.Net;

namespace TagsAPI.IntegrationTests
{
    public static class HttpClientExtensions
    {
        public static async Task<TResult> HandleGetRequestAsync<TResult>(this HttpClient client, string requestUri)
        {
            var response = await client.GetAsync(requestUri);
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            return JsonConvert.DeserializeObject<TResult>(responseContent)!;
        }

        public static async Task HandleGetRequestAndEnsurePossibleErrorsAsync<TResult>(this HttpClient client, string requestUri, params string[] expectedErrors)
        {
            if (expectedErrors.Length == 0)
            {
                await client.HandleGetRequestAndEnsureWasSuccessfulAsync<TResult>(requestUri);
            }
            else
            {
                await client.HandleGetRequestAndEnsureHasErrorsAsync(
                    requestUri,
                    expectedErrors);
            }
        }

        private static async Task<HttpResponseMessage> SendGetAsync(this HttpClient client, string requestUri)
        {
            return await client.GetAsync(requestUri);
        }

        private static async Task HandleGetRequestAndEnsureWasSuccessfulAsync<TResult>(this HttpClient client, string requestUri)
        {
            var response = await client.SendGetAsync(requestUri);
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = JsonConvert.DeserializeObject<TResult>(responseContent);

            Assert.NotNull(result);
        }

        private static async Task HandleGetRequestAndEnsureHasErrorsAsync(this HttpClient client, string requestUri, params string[] expectedErrors)
        {
            var response = await client.SendGetAsync(requestUri);
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);

            var errors = JsonConvert.DeserializeObject<string[]>(responseContent);

            Assert.Equal(expectedErrors.Length, errors!.Length);

            foreach (var expectedError in expectedErrors)
            {
                Assert.Contains(expectedError, errors);
            }
        }
    }
}
