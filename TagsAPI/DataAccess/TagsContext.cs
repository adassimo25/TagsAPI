namespace TagsAPI.DataAccess
{
    public class TagsContext(CancellationToken cancellationToken)
    {
        public CancellationToken CancellationToken { get; private set; } = cancellationToken;
    }
}
