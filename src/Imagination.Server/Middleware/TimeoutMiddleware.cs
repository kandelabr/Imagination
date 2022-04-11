using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Imagination.Middleware
{
    /// <summary>
    /// Middleware to intercept exceptions and convert them to HTTP response objects.
    /// </summary>
    public sealed class TimeoutMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeoutMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next delegate.</param>
        public TimeoutMiddleware(RequestDelegate next)
        {
            ArgumentNullException.ThrowIfNull(next, nameof(next));

            _next = next;
        }

        /// <summary>
        /// Handles exceptions during request processing
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            using (var timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(context.RequestAborted))
            {
                timeoutSource.CancelAfter(1000);
                context.RequestAborted = timeoutSource.Token;
                await _next(context);
            }
        }
    }
}
