using Newtonsoft.Json;
using System.IO.Compression;
using System.Net;
using TagsAPI.Exceptions;
using TagsAPI.Services.Interfaces;

namespace TagsAPI.Services
{
    public class ExternalAPIService<TResult> : IExternalAPIService<TResult>
        where TResult : class
    {
        private readonly HttpClient client = new();

        public async Task<TResult> GetResources(string url)
        {
            HttpResponseMessage response = new();
            string responseContent = new("<no-content>");

            try
            {
                response = await client.GetAsync(url);

                using var responseStream = await response.Content.ReadAsStreamAsync();
                using var gzipStream = new GZipStream(responseStream, CompressionMode.Decompress);
                using var reader = new StreamReader(gzipStream);
                responseContent = await reader.ReadToEndAsync();

                if (response.StatusCode != HttpStatusCode.Created)
                {
                    throw new Exception("Request failed");
                }

                var result = JsonConvert.DeserializeObject<TResult>(responseContent);

                return result!;
            }
            catch (Exception e)
            {
                var message = $"ExternalAPI error. Message: {e.Message}. Response status: {response.StatusCode}. " +
                    $"Response content: {responseContent}";
                throw new ExternalAPIException(message, e.StackTrace!);
            }
        }
    }
}
