using FluentValidation.Results;
using TagsAPI.DataAccess;

namespace TagsAPI.Validators.Common
{
    public interface IRequestValidator<TContract>
        where TContract : class
    {
        Task<ValidationResult> Validate(TContract instance, TagsContext context);
    }
}