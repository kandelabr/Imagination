using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Imagination.Configuration
{
    /// <summary>
    /// Class containing collection of valid image extension
    /// </summary>
    public class ImageExtensionConfigurationCollection
    {
        /// <summary>
        /// Collection of valid image extension
        /// </summary>
        [Required]
        public IList<ImageExtensionConfiguration> ImageExtensions { get; set; }
    }
}
