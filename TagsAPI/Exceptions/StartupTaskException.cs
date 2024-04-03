namespace TagsAPI.Exceptions
{
    public class StartupTaskException(string message, string stackTrace) : Exception(message)
    {
        public override string StackTrace { get; } = stackTrace;
    }
}
