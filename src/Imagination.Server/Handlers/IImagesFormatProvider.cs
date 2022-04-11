using Imagination.Handlers.Models;
using System.Collections.Generic;

namespace Imagination.Handlers
{
    /// <summary>
    /// Interface for providing configuration of image extension data
    /// </summary>
    public interface IImagesFormatProvider
    {
        /// <summary>
        /// Provides a list of extensions
        /// </summary>
        IList<HeaderExtension> GetExtensions();
    }
}
