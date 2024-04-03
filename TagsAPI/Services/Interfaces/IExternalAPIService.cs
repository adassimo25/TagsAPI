namespace TagsAPI.Services.Interfaces
{
    public interface IExternalAPIService<TResult>
        where TResult : class
    {
        Task<TResult> GetResources(string url);
    }
}
