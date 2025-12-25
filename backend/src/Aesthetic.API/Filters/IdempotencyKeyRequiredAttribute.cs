using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aesthetic.API.Filters
{
    public class IdempotencyKeyRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var key = context.HttpContext.Request.Headers["Idempotency-Key"].ToString();
            if (string.IsNullOrWhiteSpace(key))
            {
                context.Result = new BadRequestObjectResult(new { error = "Missing Idempotency-Key header" });
            }
        }
    }
}
