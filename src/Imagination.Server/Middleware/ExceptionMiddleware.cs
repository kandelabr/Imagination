using Imagination.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Imagination.Middleware
{
    /// <summary>
    /// Middleware to intercept exceptions and convert them to HTTP response objects.
    /// </summary>
    public sealed class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next delegate.</param>
        /// <param name="logger">The logger.</param>
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            ArgumentNullException.ThrowIfNull(next, nameof(next));
            ArgumentNullException.ThrowIfNull(next, nameof(logger));

            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Handles exceptions during request processing
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext, nameof(httpContext));

            try
            {
                await _next(httpContext).ConfigureAwait(false);
            }
            catch (HttpServerException ex)
            {
                _logger.LogError(ex, "Server error occurred");
                await ErrorResponse(httpContext, HttpStatusCode.InternalServerError, "Server error occurred").ConfigureAwait(false);
            }
            catch (HttpClientException ex)
            {
                _logger.LogWarning(ex, "Client error occurred");
                await ErrorResponse(httpContext, HttpStatusCode.BadRequest, ex.Message).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                _logger.LogError("Request was canceled");
                await ErrorResponse(httpContext, HttpStatusCode.BadRequest, "Client canceled the request").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown server error occurred");
                await ErrorResponse(httpContext, HttpStatusCode.InternalServerError, "Server error occurred").ConfigureAwait(false);
            }
        }

        private static Task ErrorResponse(HttpContext httpContext, HttpStatusCode statusCode, string errorDescription)
        {
            httpContext.Response.StatusCode = (int)statusCode;

            return httpContext.Response.WriteAsJsonAsync(errorDescription);
        }
    }
}
