using TastyBytesReact.Models.Arango;

namespace TastyBytesReact.Models
{
    /// <summary>
    /// Model representation of the database Category documents
    /// describing each dish or ingredient category.
    /// </summary>
    public class CategoryModel : IArangoDocument
    {
        /// <inheritdoc/>
        public string _id { get; set; }

        /// <inheritdoc/>
        public string _key { get; set; }

        /// <inheritdoc/>
        public string _rev { get; set; }

        /// <summary>
        /// Category name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Category type. See <see cref="CategoryType"/>.
        /// </summary>
        public CategoryType Type { get; set; }

        /// <summary>
        /// Category description.
        /// </summary>
        public string Description { get; set; }
    }
}
