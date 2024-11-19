using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ViteMontevideo_API.Presentation.Dtos.Common;

namespace ViteMontevideo_API.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context) {}

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage)
                                .ToList();

                var response = ApiResponse.UnProcessableEntity(errors);

                context.Result = new UnprocessableEntityObjectResult(response);
            }
        }
    }
}
