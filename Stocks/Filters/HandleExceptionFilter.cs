using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Stocks.WebAPI.Filters
{
    public class HandleExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ContentResult() { Content = context.Exception.Message, StatusCode = 500 };
        }
    }
}

