using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TagsAPI.Contracts.Enums;
using TagsAPI.DataAccess;
using TagsAPI.Domain;
using PREC = TagsAPI.Contracts.Requests.PaginatedRequest.ErrorCodes;

namespace TagsAPI.IntegrationTests.Tags
{
    public class TagsControllerTests : IntegrationTests<TagsControllerTests>
    {
        private readonly IList<TestTag> _tags =
        [
            new TestTag() with { Name = "Tag1", Count = 1, Share = 0.01 },
            new TestTag() with { Name = "Tag2", Count = 2, Share = 0.02 },
            new TestTag() with { Name = "Tag3", Count = 3, Share = 0.03 },
            new TestTag() with { Name = "Tag4", Count = 4, Share = 0.04 },
            new TestTag() with { Name = "Tag5", Count = 5, Share = 0.05 },
            new TestTag() with { Name = "Tag6", Count = 6, Share = 0.06 },
            new TestTag() with { Name = "Tag7", Count = 7, Share = 0.07 },
            new TestTag() with { Name = "Tag8", Count = 8, Share = 0.08 },
            new TestTag() with { Name = "Tag9", Count = 9, Share = 0.09 },
        ];

        public TagsControllerTests(WebApplicationFactory<Program> factory) : base(factory)
        {
            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var _dbContext = scopedServices.GetRequiredService<TagsDbContext>();

            _dbContext.AddRange(_tags.Select(t => Tag.Create(t.Id, t.Name, t.Count, t.Share, t.CreationDate)));
            _dbContext.SaveChanges();
        }

        [Theory]
        [InlineData(1, 2, TagsSort.Count, Order.Asc)]
        [InlineData(1, 2, TagsSort.Count, Order.Desc)]
        [InlineData(1, 2, TagsSort.Share, Order.Asc)]
        [InlineData(1, 2, TagsSort.Share, Order.Desc)]
        public async Task GetTags_ForGivenValidRequest_ReturnsSortedCollection(int page, int pageSize, TagsSort sort, Order order)
        {
            Func<TestTag, object> sortBy = sort switch
            {
                TagsSort.Count => tag => tag.Count,
                TagsSort.Share => tag => tag.Share,
                _ => throw new ArgumentException($"Invalid sort option: {sort}"),
            };
            var expectedTags = order switch
            {
                Order.Asc => _tags.OrderBy(sortBy)
                                .Skip((page - 1) * pageSize).Take(pageSize),
                Order.Desc => _tags.OrderByDescending(sortBy)
                                .Skip((page - 1) * pageSize).Take(pageSize),
                _ => throw new ArgumentException($"Invalid order option: {order}"),
            };

            var request = new TestGetTagsRequest() with { Page = page, PageSize = pageSize, Sort = sort, Order = order };

            await _client.GetTagsAsync(request, expectedTags.ToList());
        }

        [Theory]
        [InlineData(-1, 0, PREC.PageMustBePositive, PREC.PageSizeMustBePositive)]
        [InlineData(0, -1, PREC.PageMustBePositive, PREC.PageSizeMustBePositive)]
        [InlineData(0, 1, PREC.PageMustBePositive)]
        [InlineData(1, 0, PREC.PageSizeMustBePositive)]
        [InlineData(1, 1)]
        [InlineData(1, 9)]
        [InlineData(1, 10)]
        [InlineData(2, 9, PREC.PageDoesNotExist)]
        [InlineData(2, 10, PREC.PageDoesNotExist)]
        [InlineData(0, Consts.Pagination.MaxPageSize, PREC.PageMustBePositive)]
        [InlineData(1, Consts.Pagination.MaxPageSize + 1, PREC.PageSizeExceeded)]
        public async Task GetTags_ForGivenRequest_ReturnsValidResponseOrErrorCodes(int page, int pageSize, params string[] errorCodes)
        {
            var request = new TestGetTagsRequest() with { Page = page, PageSize = pageSize };

            await _client.GetTagsAsync(request, errorCodes);
        }
    }
}
