using TastyBytesReact.Models.Arango;

namespace TastyBytesReact.Models.Nodes
{
    /// <summary>
    /// Model representation of the database Category documents
    /// describing each dish or ingredient category.
    /// </summary>
    public class CategoryModel : ArangoDocument
    {
        /// <summary>
        /// Category name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Category type. See <see cref="CategoryType"/>.
        /// </summary>
        public CategoryType Type { get; set; }

        /// <summary>
        /// Category description.
        /// </summary>
        public required string Description { get; set; }
    }
}
