using Microsoft.EntityFrameworkCore;
using TagsAPI.Contracts.Requests;
using TagsAPI.DataAccess;
using TagsAPI.Domain;
using TagsAPI.Validators;

namespace TagsAPI.UnitTests.ValidatorsTests
{
    public class GetTagsRVTests
    {
        [Theory]
        [InlineData(3, 3, 0)]
        [InlineData(3, 3, 3)]
        [InlineData(3, 3, 4)]
        [InlineData(3, 3, 6)]
        public async Task Validate_WithInvalidRequestDueToDbCount_ReturnsError(int page, int pageSize, int tagsCount)
        {
            var options = new DbContextOptionsBuilder<TagsDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestTagsDb_{Guid.NewGuid()}")
                .Options;
            using var dbContext = new TagsDbContext(options);

            await SeedAsync(dbContext, tagsCount);

            var validator = new GetTagsRV(dbContext);
            var request = new GetTagsRequest { Page = page, PageSize = pageSize };

            var result = await validator.ValidateAsync(request);

            Assert.False(result.IsValid);
            Assert.Equal(PaginatedRequest.ErrorCodes.PageDoesNotExist, result.Errors.Single().ErrorCode);
        }

        [Theory]
        [InlineData(1, 3, 7)]
        [InlineData(3, 3, 7)]
        [InlineData(3, 3, 9)]
        [InlineData(3, 3, 10)]
        public async Task Validate_WithValidRequestDueToDbCount_ReturnsTrue(int page, int pageSize, int tagsCount)
        {
            var options = new DbContextOptionsBuilder<TagsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestTagsDb")
                .Options;
            using var dbContext = new TagsDbContext(options);

            await SeedAsync(dbContext, tagsCount);

            var validator = new GetTagsRV(dbContext);
            var request = new GetTagsRequest { Page = page, PageSize = pageSize };

            var result = await validator.ValidateAsync(request);

            Assert.True(result.IsValid);
        }

        private static async Task SeedAsync(TagsDbContext db, int tagsCount)
        {
            var tags = Enumerable.Range(1, tagsCount)
                .Select(i => Tag.Create(Guid.NewGuid(), $"Tag{i}", i, 0, DateTime.Now))
                .ToList();
            await db.AddRangeAsync(tags);
            await db.SaveChangesAsync();
        }
    }
}
