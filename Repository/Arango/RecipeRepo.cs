using ArangoDBNetStandard;
using TastyBytesReact.Models;
using TastyBytesReact.Models.Nodes;

namespace TastyBytesReact.Repository.Arango
{
    public class RecipeRepo
    {
        private readonly IArangoDBClient _client;
        public RecipeRepo(IArangoDBClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Gets all recipes.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RecipeModel>> GetAll()
        {
            var cursor = await _client.Cursor.PostCursorAsync<RecipeModel>(
                @"FOR doc IN Recipe
                  RETURN UNSET(doc, ""Images"")")
                .ConfigureAwait(false);
            return cursor.Result;
        }

        /// <summary>
        /// Gets all extended recipes.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RecipeModel>> GetAllExtended()
        {
            var cursor = await _client.Cursor.PostCursorAsync<ExtendedRecipeModel>(
                @"FOR doc IN Recipe
                    LET categories = (FOR v IN 1 OUTBOUND doc._id InCategory RETURN v)
                    LET ingredients = (FOR v,e IN 1 INBOUND doc._id IsContained RETURN MERGE(e, v))
                    LET allergens = (FOR ingredient IN ingredients
                    FOR v IN 1 OUTBOUND ingredient._id InCategory RETURN v
                )
                RETURN MERGE(doc,{Categories: categories},{Ingredients: ingredients}, {Allergens: allergens})")
                .ConfigureAwait(false);
            return cursor.Result;
        }

        /// <summary>
        /// Gets all recipes by category.
        /// </summary>
        /// <param name="category">Category model to filter by.</param>
        /// <returns></returns>
        //public async Task<IEnumerable<RecipeModel>> GetAll(CategoryModel category)
        //{

        //}

        /// <summary>
        /// Gets all recipes filtered by preparation time.
        /// </summary>
        /// <param name="time">Recipe preparation time to filter by.</param>
        /// <param name="comparison">Comparison type.See <see cref="ComparisonType"></see>.</param>
        /// <returns></returns>
        public async Task<IEnumerable<RecipeModel>> GetByPrepTime(int time, ComparisonType comparison)
        {
            var comparisonTypes = new Dictionary<ComparisonType, string>() {
                {ComparisonType.Equal, "==" },
                {ComparisonType.NotEqual, "!=" },
                {ComparisonType.GreaterThanOrEqual, ">=" },
                {ComparisonType.LessThanOrEqual, "<=" },
                {ComparisonType.GreaterThan, ">" },
                {ComparisonType.LessThan, "<" },
            };

            var cursor = await _client.Cursor.PostCursorAsync<RecipeModel>(
                @"FOR doc IN Recipe FILTER doc.PreparationTime " + comparisonTypes[comparison] +@" @time
                RETURN doc", new Dictionary<string, object>() {
                    { "time", time }
                })
                .ConfigureAwait(false);
            return cursor.Result;
        }
    }
}
