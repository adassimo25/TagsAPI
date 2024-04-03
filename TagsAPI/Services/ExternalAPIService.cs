using Newtonsoft.Json;
using System.IO.Compression;
using TagsAPI.Services.Interfaces;

namespace TagsAPI.Services
{
    public class ExternalAPIService<TResult>(ILogger<ExternalAPIService<TResult>> logger) : IExternalAPIService<TResult>
        where TResult : class
    {
        private readonly HttpClient client = new();
        private readonly ILogger<ExternalAPIService<TResult>> logger = logger;

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

                var result = JsonConvert.DeserializeObject<TResult>(responseContent);

                return result!;
            }
            catch (Exception e)
            {
                logger.LogError($"ExternalAPI response status: {response.StatusCode}");
                logger.LogError($"ExternalAPI response content: {responseContent}");

                throw new Exception(e.Message);
            }
        }
    }
}
