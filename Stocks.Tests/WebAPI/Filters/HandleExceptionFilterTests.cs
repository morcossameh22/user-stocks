using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Moq;
using Stocks.WebAPI.Filters;

namespace Stocks.Tests.WebAPI.Filters
{
	public class HandleExceptionFilterTests
    {
        private HandleExceptionFilter _filter;
        private ExceptionContext _context;

        public HandleExceptionFilterTests()
        {
            var httpContext = new Mock<HttpContext>();
            _filter = new HandleExceptionFilter();
            _context = new ExceptionContext(new ActionContext(
                httpContext.Object,
                new Microsoft.AspNetCore.Routing.RouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
            ), new[] { _filter });
        }

        [Fact]
        public void OnException_ShouldSetContentResultWithErrorMessageAndStatusCode()
        {
            var exceptionMessage = "An error occurred.";

            _filter = new HandleExceptionFilter();
            _context.Exception = new System.Exception(exceptionMessage);

            _filter.OnException(_context);

            Assert.NotNull(_context.Result);
            Assert.IsType<ContentResult>(_context.Result);
            ContentResult contentResult = (ContentResult) _context.Result;
            Assert.Equal(exceptionMessage, contentResult.Content);
            Assert.Equal(500, contentResult.StatusCode);
        }
    }
}

