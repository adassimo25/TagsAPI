using Moq;
using TagsAPI.Contracts.Requests;
using TagsAPI.DataAccess;
using TagsAPI.Validators;
using PREC = TagsAPI.Contracts.Requests.PaginatedRequest.ErrorCodes;

namespace TagsAPI.UnitTests.ValidatorsTests
{
    public class PaginatedRVTests
    {
        private readonly Mock<PaginatedRV<PaginatedRequest>> validator = new() { CallBase = true };

        [Theory]
        [InlineData(1, 10)]
        [InlineData(2, 20)]
        [InlineData(3, 30)]
        public async Task Validate_WithValidPageAndPageSize_ReturnsValidResult(int page, int pageSize)
        {
            var request = new Mock<PaginatedRequest>();
            request.Object.Page = page;
            request.Object.PageSize = pageSize;

            var validationResult = await validator.Object.ValidateAsync(request.Object);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(-1, 10)]
        public async Task Validate_WithInvalidPage_ReturnsPageMustBePositiveErrorCode(int page, int pageSize)
        {
            var request = new Mock<PaginatedRequest>();
            request.Object.Page = page;
            request.Object.PageSize = pageSize;

            var result = await validator.Object.ValidateAsync(request.Object);

            Assert.False(result.IsValid);
            Assert.Equal(PREC.PageMustBePositive, result.Errors.Single().ErrorCode);
        }

        [Theory]
        [InlineData(10, 0)]
        [InlineData(10, -1)]
        public async Task Validate_WithInvalidPageSize_ReturnsPageSizeMustBePositiveErrorCode(int page, int pageSize)
        {
            var request = new Mock<PaginatedRequest>();
            request.Object.Page = page;
            request.Object.PageSize = pageSize;

            var result = await validator.Object.ValidateAsync(request.Object);

            Assert.False(result.IsValid);
            Assert.Equal(PREC.PageSizeMustBePositive, result.Errors.Single().ErrorCode);
        }

        [Theory]
        [InlineData(10, Consts.Pagination.MaxPageSize + 1)]
        [InlineData(10, 10 * Consts.Pagination.MaxPageSize)]
        public async Task Validate_WithPageSizeExceedingLimit_ReturnsPageSizeExceededErrorCode(int page, int pageSize)
        {
            var request = new Mock<PaginatedRequest>();
            request.Object.Page = page;
            request.Object.PageSize = pageSize;

            var result = await validator.Object.ValidateAsync(request.Object);

            Assert.False(result.IsValid);
            Assert.Equal(PREC.PageSizeExceeded, result.Errors.Single().ErrorCode);
        }

        [Theory]
        [InlineData(0, 0, PREC.PageMustBePositive, PREC.PageSizeMustBePositive)]
        [InlineData(-1, -1, PREC.PageMustBePositive, PREC.PageSizeMustBePositive)]
        [InlineData(0, Consts.Pagination.MaxPageSize + 1, PREC.PageMustBePositive, PREC.PageSizeExceeded)]
        [InlineData(-1, Consts.Pagination.MaxPageSize + 1, PREC.PageMustBePositive, PREC.PageSizeExceeded)]
        [InlineData(0, 10 * Consts.Pagination.MaxPageSize, PREC.PageMustBePositive, PREC.PageSizeExceeded)]
        [InlineData(-1, 10 * Consts.Pagination.MaxPageSize, PREC.PageMustBePositive, PREC.PageSizeExceeded)]
        public async Task Validate_WithInvalidValues_ReturnsManyErrorCodes(int page, int pageSize, params string[] expectedErrorCodes)
        {
            var request = new Mock<PaginatedRequest>();
            request.Object.Page = page;
            request.Object.PageSize = pageSize;

            var result = await validator.Object.ValidateAsync(request.Object);

            Assert.False(result.IsValid);
            Assert.Equal(expectedErrorCodes.Length, result.Errors.Count);

            var actualErrorCodes = result.Errors.Select(error => error.ErrorCode).ToList();
            foreach (var expectedErrorCode in expectedErrorCodes)
            {
                Assert.Contains(expectedErrorCode, actualErrorCodes);
            }
        }
    }
}
