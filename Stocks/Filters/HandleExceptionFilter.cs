using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Stocks.WebAPI.Filters
{
    /* The HandleExceptionFilter class implements the IExceptionFilter interface to handle exceptions
    by returning an error message and status code. */
    public class HandleExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// This function sets the result of an exception context to a content result with the exception
        /// message and a status code of 500.
        /// </summary>
        /// <param name="ExceptionContext">ExceptionContext is a class that provides context information
        /// about an exception that occurred during the processing of an HTTP request.</param>
        public void OnException(ExceptionContext context)
        {
            context.Result = new ContentResult() { Content = context.Exception.Message, StatusCode = 500 };
        }
    }
}

