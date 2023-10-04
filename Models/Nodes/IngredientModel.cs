using TastyBytesReact.Models.Arango;

namespace TastyBytesReact.Models.Nodes
{
    /// <summary>
    /// Model representation of the database Ingredient documents
    /// describing each ingredient.
    /// </summary>
    public class IngredientModel : ArangoDocument
    {
        /// <summary>
        /// Ingredient name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ingredient description.
        /// </summary>
        public string Description { get; set; }
    }
}
