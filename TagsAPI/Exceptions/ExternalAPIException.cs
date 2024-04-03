namespace TagsAPI.Exceptions
{
    public class ExternalAPIException(string message, string stackTrace) : Exception(message)
    {
        public override string StackTrace { get; } = stackTrace;
    }
}
