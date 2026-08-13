using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Api.ErrorHandling;

public sealed class GlobalExceptionHandler
    : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler>
        _logger;


    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger =
            logger;
    }


    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // ============================================================
        // CLIENT DISCONNECTED / REQUEST CANCELLED
        // ============================================================

        if (exception is OperationCanceledException &&
            httpContext.RequestAborted.IsCancellationRequested)
        {
            return false;
        }


        // ============================================================
        // STATUS MAPPING
        // ============================================================

        var statusCode =
            exception switch
            {
                KeyNotFoundException =>
                    StatusCodes.Status404NotFound,

                UnauthorizedAccessException =>
                    StatusCodes.Status403Forbidden,

                DomainException =>
                    StatusCodes.Status400BadRequest,

                InvalidOperationException =>
                    StatusCodes.Status400BadRequest,

                _ =>
                    StatusCodes.Status500InternalServerError
            };


        // ============================================================
        // TITLE
        // ============================================================

        var title =
            statusCode switch
            {
                StatusCodes.Status400BadRequest =>
                    "Bad Request",

                StatusCodes.Status403Forbidden =>
                    "Forbidden",

                StatusCodes.Status404NotFound =>
                    "Not Found",

                StatusCodes.Status500InternalServerError =>
                    "Internal Server Error",

                _ =>
                    "Error"
            };


        // ============================================================
        // LOGGING
        // ============================================================

        if (statusCode >=
            StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(
                exception,
                "Unhandled exception. TraceId: {TraceId}",
                httpContext.TraceIdentifier);
        }
        else
        {
            _logger.LogWarning(
                "Handled exception: {ExceptionType}. Message: {Message}. TraceId: {TraceId}",
                exception.GetType().Name,
                exception.Message,
                httpContext.TraceIdentifier);
        }


        // ============================================================
        // DON'T LEAK INTERNAL 500 DETAILS
        // ============================================================

        var detail =
            statusCode ==
            StatusCodes.Status500InternalServerError
                ? "An unexpected server error occurred."
                : exception.Message;


        // ============================================================
        // PROBLEM DETAILS
        // ============================================================

        var problemDetails =
            new ProblemDetails
            {
                Status =
                    statusCode,

                Title =
                    title,

                Detail =
                    detail,

                Instance =
                    httpContext.Request.Path
            };


        problemDetails.Extensions["traceId"] =
            httpContext.TraceIdentifier;


        httpContext.Response.StatusCode =
            statusCode;

        httpContext.Response.ContentType =
            "application/problem+json";


        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);


        return true;
    }
}