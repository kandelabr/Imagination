﻿using Imagination.Handlers.Models;
using Imagination.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private const string JPG_EXTENSION = "image/jpeg";

        private readonly IExtensionBuilder _extensionBuilder;
        private readonly ILogger<ImageConverter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageConverter" /> class.
        /// </summary>
        /// <param name="extensionBuilder">The extension builder</param>
        /// <param name="logger">The logger</param>
        public ImageConverter(IExtensionBuilder extensionBuilder, ILogger<ImageConverter> logger)
        {
            ArgumentNullException.ThrowIfNull(extensionBuilder, nameof(extensionBuilder));
            ArgumentNullException.ThrowIfNull(extensionBuilder, nameof(logger));

            _extensionBuilder = extensionBuilder;
            _logger = logger;
        }


        /// <summary>
        /// Converts image into JPG format
        /// </summary>
        /// <param name="imageStream">Request image stream</param>
        /// <returns>JPG image stream</returns>
        public async Task<FileStreamResult> ConvertToJpg(Stream imageStream, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Conversion started");

            ArgumentNullException.ThrowIfNull(imageStream, nameof(imageStream));

            var extension = await _extensionBuilder.GetImageExtensionType(imageStream).ConfigureAwait(false);
            var responseStream = await GetResponseStream(imageStream, extension.Type, cancellationToken).ConfigureAwait(false);

            _logger.LogDebug($"Conversion finished, from {extension.Name} and original size {imageStream.Length} to JPG with new size {responseStream.Length}");

            return new FileStreamResult(responseStream, JPG_EXTENSION);
        }

        private async Task<Stream> GetResponseStream(Stream imageStream, ImageExtensionType extension, CancellationToken cancellationToken)
        {
            imageStream.Seek(0, SeekOrigin.Begin);

            switch (extension)
            {
                case ImageExtensionType.Jpg:
                    {
                        Stream responseStream = new MemoryStream();
                        await imageStream.CopyToAsync(responseStream, cancellationToken).ConfigureAwait(false);
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
