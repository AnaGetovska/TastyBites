using TastyBytesReact.Models.Arango;

namespace TastyBytesReact.Models.Nodes
{
    /// <summary>
    /// Model representation of the database Recipe documents
    /// describing each recipe.
    /// </summary>
    public class RecipeModel : ArangoDocument
    {
        /// <summary>
        /// Recipe name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Recipe preparation time in minutes.
        /// </summary>
        public int PreparationTime { get; set; }

        /// <summary>
        /// Number of portions for a recipe.
        /// </summary>
        public int Portions { get; set; }

        /// <summary>
        /// Recipe description including preparation process.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Recipe's main image path.
        /// </summary>
        public string DisplayImage { get; set; }
    }
}
