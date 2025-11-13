using System.Net;
using System.Text.Json;
using FluentValidation;
using Wallet.Application.Common.Exceptions;

namespace Wallet.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next; 
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); 
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception occurred");

            var response = context.Response;
            response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var result = new ErrorResponse
            {
                Message = "An unexpected error occurred.",
                Details = exception.Message
            };

            switch (exception)
            {
                case FluentValidation.ValidationException validationEx:
                    statusCode = HttpStatusCode.BadRequest;
                    result.Message = "Validation failed";
                    result.Errors = validationEx.Errors.Select(e => e.ErrorMessage).ToList();
                    break;

                case EntityNotFoundException notFoundEx:
                    statusCode = HttpStatusCode.NotFound;
                    result.Message = notFoundEx.Message;
                    break;

                case ArgumentException argEx:
                    statusCode = HttpStatusCode.BadRequest;
                    result.Message = argEx.Message;
                    break;
            }

            response.StatusCode = (int)statusCode;
            await response.WriteAsync(JsonSerializer.Serialize(result));
        }

        private class ErrorResponse
        {
            public string Message { get; set; } = string.Empty;
            public string? Details { get; set; }
            public List<string>? Errors { get; set; }
        }
    }
}
