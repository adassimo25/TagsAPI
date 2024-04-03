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
                    cmd.Page > 0 &&
                    cmd.PageSize > 0 &&
                    cmd.Page * cmd.PageSize <= await dbContext.Tags.CountAsync(cancellationToken: ct))
                .WithErrorCode(PaginatedRequest.ErrorCodes.PageDoesNotExist);
        }
    }
}
