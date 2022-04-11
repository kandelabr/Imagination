using Imagination.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Imagination.Server.Test
{
    [TestClass]
    public class CoverterUnitTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private IImageConverter _converter;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [TestInitialize]
        public void Initialize()
        {
            var serviceInjector = new DependencyInjectorForTestProjects();
            _converter = serviceInjector.GetService<IImageConverter>();
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task TestSimpleSuccessConvert()
        {
            var png = File.Open(@"Data\png.png", FileMode.Open);
            var response = await _converter.ConvertToJpg(png, new CancellationToken()).ConfigureAwait(false);

            Assert.IsNotNull(response, "Response must not be null");
            Assert.AreNotEqual(0, response.Length, "Stream length must be greater then 0");
            Assert.AreEqual(0, response.Position, "Stream position must be 0");

            await SaveFile(response, ".jpg");
        }

        private async Task SaveFile(Stream responseStream, string extension)
        {
            string fileName = $"{System.IO.Path.GetTempPath()}{Guid.NewGuid()}{extension}";

            var outStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite,
                FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan);
            await responseStream.CopyToAsync(outStream);
            await outStream.FlushAsync();

            Console.WriteLine($"File {fileName} successfully created");
        }
    }
}