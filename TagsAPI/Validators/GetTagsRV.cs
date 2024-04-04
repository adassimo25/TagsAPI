using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TagsAPI.Contracts.Requests;
using TagsAPI.DataAccess;

namespace TagsAPI.Validators
{
    public class GetTagsRV : PaginatedRV<GetTagsRequest>
    {
        public GetTagsRV(TagsDbContext dbContext) : base()
        {
            RuleFor(cmd => cmd)
                .MustAsync(async (cmd, ct) =>
                {
                    if (cmd.Page <= 0 || cmd.PageSize <= 0)
                    {
                        return true;
                    }

                    var skipped = (cmd.Page - 1) * cmd.PageSize;
                    var maxIdx = cmd.Page * cmd.PageSize;
                    var count = await dbContext.Tags.CountAsync(cancellationToken: ct);

                    return maxIdx <= count || (maxIdx > count && count > skipped);
                })
                .WithErrorCode(PaginatedRequest.ErrorCodes.PageDoesNotExist);
        }
    }
}
