using TastyBytesReact.Models.Arango;

namespace TastyBytesReact.Models.Nodes
{
    /// <summary>
    /// Model representation of all recipe properties from database including document and connected edges,
    /// describing each recipe.
    /// </summary>
    public class ExtendedRecipeModel : RecipeModel
    {
        /// <summary>
        /// Collection of all ingredients for a recipe
        /// </summary>
        public IEnumerable<IngredientModel> Ingredients { get; set; }

        /// <summary>
        /// Collection of all recipe categories
        /// </summary>
        public IEnumerable<CategoryModel> Categories { get; set; }

        /// <summary>
        /// Type of allergen which corresponds to the recipe.
        /// </summary>
        public IEnumerable<CategoryModel> Allergens { get; set; }

        /// <summary>
        /// Recipe's secondary collection of images' path.
        /// </summary>
        public string[] Images { get; set; }
    }
}
