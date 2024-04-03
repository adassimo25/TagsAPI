using FluentValidation;
using TagsAPI.Contracts.Requests;
using TagsAPI.DataAccess;
using TagsAPI.Validators.Common;

namespace TagsAPI.Validators
{
    public abstract class PaginatedRV<TContract> : RequestValidator<TContract>
        where TContract : PaginatedRequest
    {
        public PaginatedRV()
        {
            RuleFor(cmd => cmd.Page)
                .GreaterThan(0)
                .WithErrorCode(PaginatedRequest.ErrorCodes.PageMustBePositive);

            RuleFor(cmd => cmd.PageSize)
                .GreaterThan(0)
                .WithErrorCode(PaginatedRequest.ErrorCodes.PageSizeMustBePositive);
            RuleFor(cmd => cmd.PageSize)
                .LessThanOrEqualTo(Consts.Pagination.MaxPageSize)
                .WithErrorCode(PaginatedRequest.ErrorCodes.PageSizeExceeded);
        }
    }
}
