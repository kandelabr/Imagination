using Imagination.Handlers.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imagination.Handlers
{
    /// <summary>
    /// Extension builder class implementing <see cref="IExtensionBuilder"/> interface
    /// </summary>
    public class ExtensionBuilder : IExtensionBuilder
    {
        private readonly IImagesFormatProvider _imagesFormatProvider;

        private IList<HeaderExtension> _extensions;
        private int _maxSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionBuilder" /> class.
        /// </summary>
        /// <param name="imagesFormatProvider">The format provider</param>
        public ExtensionBuilder(IImagesFormatProvider imagesFormatProvider)
        {
            ArgumentNullException.ThrowIfNull(imagesFormatProvider, nameof(imagesFormatProvider));

            _imagesFormatProvider = imagesFormatProvider;
        }

        /// <summary>
        /// Get extension image type
        /// </summary>
        public async Task<ImageExtension> GetImageExtensionType(Stream stream, CancellationToken cancellationToken)
        {
            if (_extensions == null)
            {
                _extensions = _imagesFormatProvider.GetExtensions();
                _maxSize = _extensions.Select(x => x.ExtensionData.Length).Max();
            }

            var bytes = await GetMaxImageHeaderBytes(stream, cancellationToken).ConfigureAwait(false);
            return GetImageExtensionType(bytes);
        }

        private async Task<byte[]> GetMaxImageHeaderBytes(Stream stream, CancellationToken cancellationToken)
        {
            byte[] extensionBytes = new byte[_maxSize];

            if (stream.Length >= _maxSize)
            {
                await stream.ReadAsync(extensionBytes, 0, _maxSize, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await stream.ReadAsync(extensionBytes, cancellationToken).ConfigureAwait(false);
                extensionBytes = extensionBytes.Take((int)stream.Length).ToArray();
            }

            return extensionBytes;
        }

        private ImageExtension GetImageExtensionType(byte[] bytes)
        {
            foreach (var imageExtension in _extensions)
            {
                if (bytes.Length >= imageExtension.ExtensionData.Length)
                {
                    bool isValid = true;
                    foreach (var configurataion in imageExtension.CompareExtensionConfigurations)
                    {
                        isValid &= bytes.Skip(configurataion.Start)
                            .Take(configurataion.Size)
                            .SequenceEqual(imageExtension.ExtensionData.Skip(configurataion.Start).Take(configurataion.Size));
                    }
                    if (isValid)
                    {
                        return imageExtension.Extension;
                    }
                }
            }

            return ImageExtension.FromInvalidType();
        }
    }
}
