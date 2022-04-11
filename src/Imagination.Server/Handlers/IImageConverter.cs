using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Imagination.Handlers
{
    /// <summary>
    /// Interface that converts image format
    /// </summary>
    public interface IImageConverter
    {
        /// <summary>
        /// Converts image into JPG format
        /// </summary>
        /// <param name="imageStream">Request image stream</param>
        /// <returns>JPG image stream</returns>
        Task<FileStreamResult> ConvertToJpg(Stream imageStream, CancellationToken cancellationToken);
    }
}
