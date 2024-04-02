using FluentValidation;
using FluentValidation.Results;
using TagsAPI.DataAccess;

namespace TagsAPI.Validators.Common
{
    public abstract class RequestValidator<TContract>
        : AbstractValidator<TContract>, IRequestValidator<TContract>
        where TContract : class
    {
        protected readonly TagsDbContext? dbContext;
        protected TagsContext context = new(default);

        public RequestValidator(TagsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public RequestValidator()
        {
        }

        async Task<ValidationResult> IRequestValidator<TContract>.Validate(TContract instance, TagsContext context)
        {
            this.context = context;

            return await ValidateAsync(instance);
        }
    }
}