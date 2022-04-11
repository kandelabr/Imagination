using System.Collections.Generic;

namespace Imagination.Handlers.Models
{
    /// <summary>
    /// Extension class with header data
    /// </summary>
    public class HeaderExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderExtension" /> class.
        /// </summary>
        /// <param name="extensionData">Byte array used to identify extension</param>
        /// <param name="compareExtensionConfigurations">Identification configuration</param>
        /// <param name="extension">The extension</param>
        public HeaderExtension(byte[] extensionData, IList<CompareExtensionConfiguration> compareExtensionConfigurations, ImageExtension extension)
        {
            ExtensionData = extensionData;
            CompareExtensionConfigurations = compareExtensionConfigurations;
            Extension = extension;
        }

        /// <summary>
        /// Byte data indicating extension
        /// </summary>
        /// <example>
        /// [0xFF, 0xD8] => JPG
        /// </example>
        public byte[] ExtensionData { get; private set; }

        /// <summary>
        /// Compare configurations
        /// </summary>
        public IList<CompareExtensionConfiguration> CompareExtensionConfigurations { get; private set; }

        /// <summary>
        /// Extension object
        /// </summary>
        public ImageExtension Extension { get; private set; }
    }
}
