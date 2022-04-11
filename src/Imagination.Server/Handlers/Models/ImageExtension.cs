namespace Imagination.Handlers.Models
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
        ValidImage,
        /// <summary>
        /// Invalid image type (TXT, MP3, ...)
        /// </summary>
        Invalid
    }

    /// <summary>
    /// Image extension class
    /// </summary>
    public class ImageExtension
    {
        /// <summary>
        /// Extension name
        /// </summary>
        /// <example>
        /// PNG
        /// </example>
        public string Name { get; private set; }

        /// <summary>
        /// Image extension type
        /// </summary>
        public ImageExtensionType Type { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageExtension" /> class.
        /// </summary>
        /// <param name="name">Extension name</param>
        /// <param name="type">Extension type</param>
        public ImageExtension(string name, ImageExtensionType type)
        {
            Name = name;
            Type = type;
        }
    }
}
