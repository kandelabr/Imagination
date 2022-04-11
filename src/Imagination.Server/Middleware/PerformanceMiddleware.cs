using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Imagination.Middleware
{
    /// <summary>
    /// Middleware to monitor HTTP call performance.
    /// </summary>
    public sealed class PerformanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next delegate.</param>
        /// <param name="logger">The logger.</param>
        public PerformanceMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            ArgumentNullException.ThrowIfNull(next, nameof(next));
            ArgumentNullException.ThrowIfNull(next, nameof(logger));

            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Monitors duration of HTTP call
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext, nameof(httpContext));

            Stopwatch stopwatch = Stopwatch.StartNew();
            await _next(httpContext).ConfigureAwait(false);
            stopwatch.Stop();
            _logger.LogInformation($"Conversion time in milliseconds: {stopwatch.ElapsedMilliseconds}");
        }
    }
}
