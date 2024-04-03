namespace TagsAPI.Contracts.Requests
{
    public abstract class PaginatedRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public static class ErrorCodes
        {
            public const string PageMustBePositive = "Page must be positive";
            public const string PageSizeMustBePositive = "Page size must be positive";
            public const string PageSizeExceeded = "Page size exceeded";
            public const string PageDoesNotExist = "Page does not exist";
        }
    }
}
