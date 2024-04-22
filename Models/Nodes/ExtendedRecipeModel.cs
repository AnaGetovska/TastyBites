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

        /// <summary>
        /// Recipe's rating.
        /// </summary>
        public float Rating { get; set; }

        /// <summary>
        /// Count of voted users.
        /// </summary>
        public float RatingCount { get; set; }

        /// <summary>
        /// Sets to true if the recipe is liked by current user
        /// </summary>
        public bool LikedBy { get; set; }
    }
}
