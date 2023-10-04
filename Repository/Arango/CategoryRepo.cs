using ArangoDBNetStandard;
using TastyBytesReact.Models;
using TastyBytesReact.Models.Nodes;

namespace TastyBytesReact.Repository.Arango
{
    public class CategoryRepo
    {
        private readonly IArangoDBClient _client;
        public CategoryRepo(IArangoDBClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CategoryModel>> GetAll()
        {
            var cursor = await _client.Cursor.PostCursorAsync<CategoryModel>(
                @"FOR doc IN Category RETURN doc")
                .ConfigureAwait(false);
            return cursor.Result;
        }

        /// <summary>
        /// Gets all categories by type. See <see cref="CategoryType"/> for the available types.
        /// </summary>
        /// <param name="type">Category type to filter by.</param>
        /// <returns></returns>
        public async Task<IEnumerable<CategoryModel>> GetAll(CategoryType type)
        {
            var cursor = await _client.Cursor.PostCursorAsync<CategoryModel>(
                @"FOR doc IN Category FILTER doc.Type == @type
                RETURN doc", new Dictionary<string, object>() {
                    { "type", type.ToString() }
                })
                .ConfigureAwait(false);
            return cursor.Result;
        }

        /// <summary>
        /// Gets a category by key.
        /// </summary>
        /// <param name="key">Category key to filter by.</param>
        /// <returns></returns>
        public async Task<IEnumerable<CategoryModel>> GetByKey(string key)
        {
            var cursor = await _client.Cursor.PostCursorAsync<CategoryModel>(
                @"FOR doc IN Category FILTER doc._key == @key
                RETURN doc", new Dictionary<string, object>() {
                    { "key", key.ToString() }
                })
                .ConfigureAwait(false);
            return cursor.Result;
        }
    }
}
