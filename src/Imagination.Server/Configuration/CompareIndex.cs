using System.ComponentModel.DataAnnotations;

namespace Imagination.Configuration
{
    /// <summary>
    /// Compare index class
    /// </summary>
    public class CompareIndex
    {
        /// <summary>
        /// Index start
        /// </summary>
        [Required]
        public int Start { get; set; }

        /// <summary>
        /// Compare size
        /// </summary>
        [Required]
        public int Size { get; set; }
    }
}
