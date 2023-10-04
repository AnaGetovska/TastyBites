using ArangoDBNetStandard;
using TastyBytesReact.Models;
using TastyBytesReact.Models.Nodes;

namespace TastyBytesReact.Repository.Arango
{
    public class IngredientRepo
    {
        private readonly IArangoDBClient _client;
        public IngredientRepo(IArangoDBClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Gets all ingredients.
        /// </summary>
        public async Task<IEnumerable<IngredientModel>> GetAll()
        {
            var cursor = await _client.Cursor.PostCursorAsync<IngredientModel>(
                @"FOR doc IN Ingredient RETURN doc")
                .ConfigureAwait(false);
            return cursor.Result;
        }

        /// <summary>
        /// Gets all ingredients including all related categories.
        /// </summary>
        public async Task<IEnumerable<ExtendedIngredientModel>> GetAllExtended()
        {
            var cursor = await _client.Cursor.PostCursorAsync<ExtendedIngredientModel>(
                @"FOR doc IN Ingredient
                    LET categories = (FOR v IN 1 OUTBOUND doc._id InCategory RETURN v)
                RETURN MERGE({Categories: categories}, doc)")
                .ConfigureAwait(false);
            return cursor.Result;
        }
    }
}
