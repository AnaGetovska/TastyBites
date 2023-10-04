using TastyBytesReact.Models.Arango;

namespace TastyBytesReact.Models.Nodes
{
    /// <summary>
    /// Model representation of all ingredient properties from database including document and connected edges,
    /// describing each ingredient.
    /// </summary>
    public class ExtendedIngredientModel : IngredientModel
    {

        /// <summary>
        /// Collection of all ingredient categories.
        /// </summary>
        public IEnumerable<CategoryModel> Categories { get; set; }
    }
}
