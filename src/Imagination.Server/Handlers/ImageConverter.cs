using Imagination.Handlers.Models;
using Imagination.Models;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Imagination.Handlers
{
    /// <summary>
    /// Converter class converting image format by implementing <see cref="IImageConverter"/> interface
    /// </summary>
    public class ImageConverter : IImageConverter
    {

        private readonly IExtensionBuilder _extensionBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageConverter" /> class.
        /// </summary>
        /// <param name="extensionBuilder">The extension builder</param>
        public ImageConverter(IExtensionBuilder extensionBuilder)
        {
            ArgumentNullException.ThrowIfNull(extensionBuilder, nameof(extensionBuilder));

            _extensionBuilder = extensionBuilder;
        }


        /// <summary>
        /// Converts image into JPG format
        /// </summary>
        /// <param name="imageStream">Request image stream</param>
        /// <returns>JPG image stream</returns>
        public async Task<Stream> ConvertToJpg(Stream imageStream, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(imageStream, nameof(imageStream));

            var extension = await _extensionBuilder.GetImageExtensionType(imageStream, cancellationToken).ConfigureAwait(false);
            var responseStream = await GetResponseStream(imageStream, extension.Type, cancellationToken).ConfigureAwait(false);
            return responseStream;
        }

        private static async Task<Stream> GetResponseStream(Stream imageStream, ImageExtensionType extension, CancellationToken cancellationToken)
        {
            imageStream.Seek(0, SeekOrigin.Begin);

            switch (extension)
            {
                case ImageExtensionType.Jpg:
                    {
                        Stream responseStream = new MemoryStream();
                        await imageStream.CopyToAsync(responseStream, 81920, cancellationToken).ConfigureAwait(false);
                        responseStream.Seek(0, SeekOrigin.Begin);
                        return responseStream;
                    }
                case ImageExtensionType.ValidImage:
                    {
                        Stream responseStream = new MemoryStream();
                        Image responseImage = await Image.LoadAsync(imageStream, cancellationToken).ConfigureAwait(false);
                        await responseImage.SaveAsJpegAsync(responseStream, cancellationToken).ConfigureAwait(false);
                        responseStream.Seek(0, SeekOrigin.Begin);
                        return responseStream;
                    }
                case ImageExtensionType.Invalid:
                    throw new HttpClientException("Invalid file type sent to conversion");
                default:
                    throw new HttpServerException("Unexpected extension type received");
            }
        }
    }
}
