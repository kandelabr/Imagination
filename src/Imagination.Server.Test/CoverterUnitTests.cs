using Imagination.Handlers;
using Imagination.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imagination.Server.Test
{
    [TestClass]
    public class CoverterUnitTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private IImageConverter _converter;
        private IExtensionBuilder _extensionBuilder;
        private CancellationToken _cancellationToken;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [TestInitialize]
        public void Initialize()
        {
            var serviceInjector = new DependencyInjectorForTestProjects();
            _converter = serviceInjector.GetService<IImageConverter>();
            _extensionBuilder = serviceInjector.GetService<IExtensionBuilder>();
            _cancellationToken = new CancellationToken();
        }

        #region Tests

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task TestSimpleSuccessConvert()
        {
            using var png = File.Open(@"Data\png.png", FileMode.Open);
            using var response = await _converter.ConvertToJpg(png, _cancellationToken).ConfigureAwait(false);

            Assert.IsNotNull(response, "Response must not be null");
            Assert.AreNotEqual(0, response.Length, "Stream length must be greater then 0");
            Assert.AreEqual(0, response.Position, "Stream position must be 0");
            await AssertIsJpg(response);

            //await SaveFile(response, ".jpg");
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task TestAllSuccessConversions()
        {
            var invalidFiles = new string[] { "invalid.png", "text.txt", "text.png" };

            foreach (var filePath in Directory.GetFiles("Data"))
            {
                if (invalidFiles.Any(x => filePath.EndsWith(x)))
                    continue;

                using var image = File.Open(filePath, FileMode.Open);
                using var response = await _converter.ConvertToJpg(image, _cancellationToken).ConfigureAwait(false);

                Assert.IsNotNull(response, "Response must not be null");
                Assert.AreNotEqual(0, response.Length, "Stream length must be greater then 0");
                Assert.AreEqual(0, response.Position, "Stream position must be 0");
                await AssertIsJpg(response);

                //await SaveFile(response, ".jpg");
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task TestAllSuccessConversionsInParallel()
        {
            var invalidFiles = new string[] { "invalid.png", "text.txt", "text.png" };

            var tasks = new List<(Stream, Task<Stream>)>();

            foreach (var filePath in Directory.GetFiles("Data"))
            {
                if (invalidFiles.Any(x => filePath.EndsWith(x)))
                    continue;

                tasks.Add(CallConvert(filePath));
            }

            var responseList = await Task.WhenAll(tasks.Select(x => x.Item2)).ConfigureAwait(false);

            foreach (var response in responseList)
            {
                Assert.IsNotNull(response, "Response must not be null");
                Assert.AreNotEqual(0, response.Length, "Stream length must be greater then 0");
                Assert.AreEqual(0, response.Position, "Stream position must be 0");
                await AssertIsJpg(response);

                //await SaveFile(response, ".jpg");
            }

            foreach (var stream in tasks.Select(x => x.Item1))
                stream.Dispose();
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task TestAllFailedConversions()
        {
            var invalidFiles = new string[] { "invalid.png" };

            foreach (var filePath in Directory.GetFiles("Data"))
            {
                if (invalidFiles.Any(x => filePath.EndsWith(x)))
                {
                    try
                    {
                        using var image = File.Open(filePath, FileMode.Open);
                        await _converter.ConvertToJpg(image, _cancellationToken).ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    Assert.Fail("Exception should have been thrown");
                }
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task TestAllInvalidFormatConversions()
        {
            var invalidFiles = new string[] { "text.txt", "text.png" };

            foreach (var filePath in Directory.GetFiles("Data"))
            {
                if (invalidFiles.Any(x => filePath.EndsWith(x)))
                {
                    using var image = File.Open(filePath, FileMode.Open);
                    await Assert.ThrowsExceptionAsync<HttpClientException>(async () => await _converter.ConvertToJpg(image, _cancellationToken).ConfigureAwait(false), "HttpClientException should have been thrown");
                }
            }
        }

        #endregion Tests

        #region Helper

        private (Stream, Task<Stream>) CallConvert(string filePath)
        {
            var image = File.Open(filePath, FileMode.Open);
            return (image, _converter.ConvertToJpg(image, _cancellationToken));
        }

        private async Task SaveFile(Stream responseStream, string extension)
        {
            responseStream.Seek(0, SeekOrigin.Begin);
            string fileName = $"{System.IO.Path.GetTempPath()}{Guid.NewGuid()}{extension}";

            var outStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite,
                FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan);
            await responseStream.CopyToAsync(outStream);
            await outStream.FlushAsync();
            outStream.Seek(0, SeekOrigin.Begin);

            Console.WriteLine($"File {fileName} successfully created");
        }

        private async Task AssertIsJpg(Stream stream)
        {
            var extension = await _extensionBuilder.GetImageExtensionType(stream, _cancellationToken).ConfigureAwait(false);
            Assert.AreEqual(Handlers.Models.ImageExtensionType.Jpg, extension.Type, "Response must be of type JPG");
        }

        #endregion Helper
    }
}