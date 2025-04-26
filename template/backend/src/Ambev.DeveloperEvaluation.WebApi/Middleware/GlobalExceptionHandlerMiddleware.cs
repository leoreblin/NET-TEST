using System.Net;
using System.Text.Json;
using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware;

/// <summary>
/// Represents the global exception handler middleware.
/// </summary>
public sealed class GlobalExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExceptionHandlerMiddleware"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles the specified <see cref="Exception"/> for the specified <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="context">The HTTP Context.</param>
    /// <param name="ex">The exception.</param>
    /// <returns>The HTTP response that is modified based on the exception.</returns>
    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        (HttpStatusCode statusCode, IReadOnlyCollection<Error> errors) = GetHttpStatusCodeAndErrors(ex);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        string response = JsonSerializer.Serialize(
            new ApiErrorResponse(errors),
            JsonSerializerOptions
        );

        await context.Response.WriteAsync(response);
    }

    /// <summary>
    /// Gets the HTTP status code and errors based on the exception type.
    /// </summary>
    /// <param name="exception">The exception that has occurred.</param>
    /// <returns>The HTTP status code and collection of errors for the specified exception.</returns>
    private static (HttpStatusCode statusCode, IReadOnlyCollection<Error> errors) GetHttpStatusCodeAndErrors(Exception exception) 
        => exception switch
        {
            ValidationException validationException => (
                HttpStatusCode.BadRequest, 
                validationException.Errors.Distinct().Select(e => new Error(e.ErrorCode, e.ErrorMessage)).ToArray()
            ),

            DomainException domainException => (
                HttpStatusCode.UnprocessableEntity,
                new[] { new Error("Domain.Error", domainException.Message) }
            ),

            UnauthorizedAccessException unauthorizedException => (
                HttpStatusCode.Unauthorized,
                new[] { new Error("Unauthorized", unauthorizedException.Message) }
            ),

            KeyNotFoundException notFoundException => (
                HttpStatusCode.NotFound,
                new[] { new Error("NotFound", notFoundException.Message) }
            ),

            _ => (HttpStatusCode.InternalServerError, new[]
            { 
                ApiErrors.ServerError, 
                new Error("Exception", exception.Message),
                new Error("InnerException", exception.InnerException?.Message ?? string.Empty)
            })
        };
}
