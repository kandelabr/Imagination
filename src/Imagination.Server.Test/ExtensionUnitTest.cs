using Imagination.Handlers;
using Imagination.Handlers.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imagination.Server.Test
{
    [TestClass]
    public class ExtensionUnitTest
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private IExtensionBuilder _extensionBuilder;
        private CancellationToken _cancellationToken;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [TestInitialize]
        public void Initialize()
        {
            var serviceInjector = new DependencyInjectorForTestProjects();
            _extensionBuilder = serviceInjector.GetService<IExtensionBuilder>();
            _cancellationToken = new CancellationToken();
        }

        #region Tests

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task TestAllExtensions()
        {
            var invalidFiles = new string[] { "text.txt", "text.png" };
            var jpgFiles = new string[] { "big.jpg", "jfif.jfif", "jpeg.jpg", "small.jpg" };

            foreach (var filePath in Directory.GetFiles("Data"))
            {
                using var image = File.Open(filePath, FileMode.Open);
                var extension = await _extensionBuilder.GetImageExtensionType(image, _cancellationToken);

                Assert.IsNotNull(extension, "Extension must not be null");

                if (invalidFiles.Any(x => filePath.EndsWith(x)))
                    Assert.AreEqual(ImageExtensionType.Invalid, extension.Type, "Extension must be of invalid type");
                else if (jpgFiles.Any(x => filePath.EndsWith(x)))
                    Assert.AreEqual(ImageExtensionType.Jpg, extension.Type, "Extension must be of jpg type");
                else
                    Assert.AreEqual(ImageExtensionType.ValidImage, extension.Type, "Extension must be of valid type");
            }
        }

        #endregion Tests
    }
}