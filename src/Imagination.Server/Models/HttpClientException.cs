using System;

namespace Imagination.Models
{
    /// <summary>
    /// Client exception class
    /// </summary>
    public class HttpClientException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientException" /> class.
        /// </summary>
        /// <param name="message">The error message</param>
        public HttpClientException(string message) : base(message) { }
    }
}
