namespace TastyBytesReact.Models.Nodes
{
    public class RecipeIngredientModel : IngredientModel
    {
        /// <summary>
        /// Quantity of ingredient contained in a recipe.
        /// </summary>
        public float Quantity { get; set; }

        /// <summary>
        /// Measurement unit of quantity.
        /// </summary>
        public string MeasurementUnit { get; set; }
    }
}
