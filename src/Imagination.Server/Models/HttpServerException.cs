using System;

namespace Imagination.Models
{
    /// <summary>
    /// Server exception class
    /// </summary>
    public class HttpServerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServerException" /> class.
        /// </summary>
        /// <param name="message">The error message</param>
        public HttpServerException(string message) : base(message) { }
    }
}
