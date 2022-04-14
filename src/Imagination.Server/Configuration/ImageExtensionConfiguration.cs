using System.ComponentModel.DataAnnotations;

namespace Imagination.Configuration
{
    /// <summary>
    /// Image extension type enum
    /// </summary>
    public enum ImageExtensionType
    {
        /// <summary>
        /// JPG, JPEG, JFIF type
        /// </summary>
        Jpg,
        /// <summary>
        /// Other valid image type (PNG, GIF...)
        /// </summary>
        ValidImage
    }

    /// <summary>
    /// Image extension configuration
    /// </summary>
    /// <remarks>
    /// Full list of types can be found at: https://en.wikipedia.org/wiki/List_of_file_signatures
    /// </remarks>
    public class ImageExtensionConfiguration
    {
        /// <summary>
        /// Extension name
        /// </summary>
        /// <example>
        /// PNG
        /// </example>
        [Required]
        public string ExtensionName { get; set; }

        /// <summary>
        /// Extension type
        /// </summary>
        [Required]
        public ImageExtensionType ExtensionType { get; set; }

        /// <summary>
        /// Byte data indicating extension
        /// </summary>
        /// <example>
        /// [0xFF, 0xD8] => JPG
        /// </example>
        [Required]
        public byte[] ExtensionData { get; set; }

        /// <summary>
        /// Compare index data
        /// </summary>
        [Required]
        public CompareIndex[] CompareIndexCollection { get; set; }
    }
}
