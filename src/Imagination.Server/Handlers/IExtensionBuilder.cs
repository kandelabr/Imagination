using Imagination.Handlers.Models;
using System.IO;
using System.Threading.Tasks;

namespace Imagination.Handlers
{
    /// <summary>
    /// Interface for building image extension
    /// </summary>
    public interface IExtensionBuilder
    {
        /// <summary>
        /// Get extension image type
        /// </summary>
        Task<ImageExtension> GetImageExtensionType(Stream stream);

    }
}
