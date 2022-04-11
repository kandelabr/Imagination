using Imagination.Handlers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Imagination.Controllers
{
    /// <summary>
    /// Converter controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertController : ControllerBase
    {
        private const string JPG_EXTENSION = "image/jpeg";

        private readonly IImageConverter _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertController" /> class.
        /// </summary>
        /// <param name="converter">The convert handler</param>
        public ConvertController(IImageConverter converter)
        {
            ArgumentNullException.ThrowIfNull(converter, nameof(converter));

            _converter = converter;
        }

        /// <summary>
        /// Converts image to JPG format
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Route("/convert")]
        [ProducesResponseType(statusCode: 200, type: typeof(FileStreamResult))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        [ProducesResponseType(statusCode: 500, type: typeof(string))]
        public async Task Convert(CancellationToken cancellationToken)
        {
            using var response = await _converter.ConvertToJpg(Request.Body, cancellationToken).ConfigureAwait(false);
            this.Response.ContentType = JPG_EXTENSION;
            await response.CopyToAsync(this.Response.Body);
        }
    }
}
