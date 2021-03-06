﻿using Hahn.ApplicatonProcess.December2020.Domain.Infrastructure;
using Hahn.ApplicatonProcess.December2020.Domain.Models;
using Hahn.ApplicatonProcess.December2020.Web.Contracts.V1.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.December2020.Web.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidationFilters : IAsyncActionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Before the controller 
            if (!context.ModelState.IsValid)
            {
                var errorsInModelStates = context.ModelState
                    .Where(c => c.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(c => c.ErrorMessage)).ToArray();

                var errorResponse = new ErrorResponse();

                foreach (var error in errorsInModelStates)
                {
                    foreach (var subError in error.Value)
                    {
                        var errorModel = new ErrorModel()
                        {
                            FieldName = error.Key,
                            Message = subError
                        };
                        errorResponse.Errors.Add(errorModel);
                    }
                }

                var apiResponse = new ApiResponse();
                apiResponse.Status = ApplicationStatusCode.BadRequest;
                apiResponse.Message = "Bad Request";
                apiResponse.Data = errorResponse;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new BadRequestObjectResult(apiResponse);
                return;
            }
            await next();
        }
    }
}
