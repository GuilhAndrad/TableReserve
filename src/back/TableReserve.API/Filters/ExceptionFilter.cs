using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TableReserve.Communication.Responses;
using TableReserve.Exception;
using TableReserve.Exception.ExceptionsBase;

namespace TableReserve.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is TableReserveException tableReserveException)
        {
            context.HttpContext.Response.StatusCode = (int)tableReserveException.StatusCode();

            context.Result = new ObjectResult(new ErrorResponse(tableReserveException.GetErrorMessages()));
        }
        else
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            context.Result = new ObjectResult(new ErrorResponse(MessagesExceptionResource.UNKNOWN_ERROR));
        }
    }
}
