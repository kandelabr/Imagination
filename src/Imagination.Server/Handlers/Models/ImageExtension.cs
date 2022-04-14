using System;

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
        /// Private constructor of the <see cref="ImageExtension" /> class.
        /// </summary>
        /// <param name="name">Extension name</param>
        /// <param name="type">Extension type</param>
        private ImageExtension(string name, ImageExtensionType type)
        {
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageExtension" /> class.
        /// </summary>
        /// <param name="name">Extension name</param>
        /// <param name="type">Extension type</param>
        /// <exception cref="ArgumentException">Thrown when type has value of Invalid</exception>
        public static ImageExtension FromValidType(string name, ImageExtensionType type)
        {
            if (type == ImageExtensionType.Invalid)
                throw new ArgumentException(nameof(type));

            return new ImageExtension(name, type);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageExtension" /> class with Invalid type value.
        /// </summary>
        public static ImageExtension FromInvalidType()
        {
            return new ImageExtension(null, ImageExtensionType.Invalid);
        }
    }
}
