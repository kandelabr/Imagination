using Imagination.Handlers.Models;
using Imagination.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagination.Handlers
{
    /// <summary>
    /// Class provides image format configuration from configuration file by implementing <see cref="IImagesFormatProvider"/> interface
    /// </summary>
    public class ConfigImagesFormatProvider : IImagesFormatProvider
    {
        private readonly IList<Configuration.ImageExtensionConfiguration> _imageExtensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigImagesFormatProvider" /> class.
        /// </summary>
        /// <param name="config">The configuration</param>
        public ConfigImagesFormatProvider(IOptions<Configuration.ImageExtensionConfigurationCollection> config)
        {
            ArgumentNullException.ThrowIfNull(config?.Value, nameof(config));

            _imageExtensions = config.Value.ImageExtensions;
        }

        /// <summary>
        /// Provides a list of extensions
        /// </summary>
        public IList<HeaderExtension> GetExtensions()
        {
            var extensions = new List<HeaderExtension>();
            foreach (var extension in _imageExtensions)
            {
                extensions.Add(new HeaderExtension(extension.ExtensionData,
                    extension.CompareIndexCollection.Select(x => new CompareExtensionConfiguration(x.Start, x.Size)).ToList(),
                    ImageExtension.FromValidType(extension.ExtensionName, Map(extension.ExtensionType))));
            }

            return extensions;
        }

        private static ImageExtensionType Map(Configuration.ImageExtensionType extensionType) => extensionType switch
        {
            Configuration.ImageExtensionType.Jpg => ImageExtensionType.Jpg,
            Configuration.ImageExtensionType.ValidImage => ImageExtensionType.ValidImage,
            _ => throw new HttpServerException($"Server mapping exception from {extensionType} type.")
        };
    }
}
