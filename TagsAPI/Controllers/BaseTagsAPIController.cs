using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TagsAPI.DataAccess;
using TagsAPI.Validators.Common;

namespace TagsAPI.Controllers
{
    public class BaseTagsAPIController(IServiceProvider serviceProvider) : ControllerBase()
    {
        private readonly IServiceProvider serviceProvider = serviceProvider;

        public ObjectResult? ValidateRequest<TContract>(object request, CancellationToken cancellationToken = default)
            where TContract : class
        {
            var validator = serviceProvider.GetService(typeof(IRequestValidator<TContract>));

            ValidationResult vResult = new();
            Task.Run(async () =>
            {
                vResult = await ((IRequestValidator<TContract>)validator!)
                    .Validate((TContract)request, new TagsContext(cancellationToken));
            }, cancellationToken)
                .Wait(cancellationToken);

            if (!vResult.IsValid)
            {
                return StatusCode(
                    (int)HttpStatusCode.UnprocessableEntity,
                    vResult.Errors.Select(e => e.ErrorCode).ToList());
            }

            return null;
        }
    }
}
