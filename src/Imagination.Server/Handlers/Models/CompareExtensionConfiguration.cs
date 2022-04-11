namespace Imagination.Handlers.Models
{
    /// <summary>
    /// Compare configuration class
    /// </summary>
    public class CompareExtensionConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompareExtensionConfiguration" /> class.
        /// </summary>
        /// <param name="start">Start index of comparison</param>
        /// <param name="logger">Size of the comparison</param>
        public CompareExtensionConfiguration(int start, int size)
        {
            Start = start;
            Size = size;
        }

        /// <summary>
        /// Index start
        /// </summary>
        public int Start { get; private set; }

        /// <summary>
        /// Compare size
        /// </summary>
        public int Size { get; private set; }
    }
}
