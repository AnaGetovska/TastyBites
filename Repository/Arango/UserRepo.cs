using ArangoDBNetStandard;
using TastyBytesReact.Models;
using TastyBytesReact.Models.Edges;
using TastyBytesReact.Models.Nodes;

namespace TastyBytesReact.Repository.Arango
{
    public class UserRepo
    {
        private readonly IArangoDBClient _client;
        public UserRepo(IArangoDBClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Creates new user in the DB.
        /// </summary>
        /// <returns></returns>
        public async Task CreateUser(UserModel userData)
        {
            var cursor = await _client.Cursor.PostCursorAsync<UserModel>(
               @"FOR doc IN User
                    FILTER doc.Username == @username
                    RETURN doc",
               new Dictionary<string, object>()
               {
                   {"username", userData.Username },
               })
               .ConfigureAwait(false);

            if(cursor.Count >= 1) {
                throw new ArgumentException("User already exists.");
            }

            await _client.Document.PostDocumentAsync("User", userData);
        
        }

        /// <summary>
        /// Removes given user.
        /// </summary>
        /// <returns></returns>
        public async Task RemoveUser(UserModel userData)
        {
            var cursor = await _client.Cursor.PostCursorAsync<RecipeModel>(
              @"FOR doc IN User
                    FILTER doc.Username == @username && doc.Password == @password
                    REMOVE doc IN User",
               new Dictionary<string, object>()
               {
                   {"name", userData.Username },
               })
               .ConfigureAwait(false);
        }

        /// <summary>
        /// Search for all user with given username.
        /// </summary>
        /// <returns><see cref="UserModel"/> if the user exists or <see cref="Array.Empty{T}"/></returns>
        public async Task<UserModel> GetUserByUsername(string username)
        {
            var cursor = await _client.Cursor.PostCursorAsync<UserModel>(
               @"FOR doc IN User
                    FILTER doc.Username == @username
                    RETURN doc",
               new Dictionary<string, object>()
               {
                   {"username", username },
               })
               .ConfigureAwait(false);
            return cursor.Result.FirstOrDefault();
        }

        /// <summary>
        /// Get weekly menu for user by key.
        /// </summary>
        public async Task<IEnumerable<InMenuModel>> GetWeeklyMenuByUserKey(string userKey)
        {
            var cursor = await _client.Cursor.PostCursorAsync<InMenuModel>(
               @"FOR e IN InMenu
                    FILTER e._to == CONCAT(""User/"", @key)
                    RETURN e",
               new Dictionary<string, object>()
               {
                   {"key", userKey },
               })
               .ConfigureAwait(false);
            return cursor.Result;
        }

        /// <summary>
        /// Set weekly menu for user by key.
        /// </summary>
        public async Task SetWeeklyMenuByUserKey(string userKey, string recipeKey, DayEnum[] days)
        {
            var recipeMenuItems = (await GetWeeklyMenuByUserKey(userKey)).Where(i => i._from == "Recipe/" + recipeKey).ToList();

            var daysToAdd = days.Where(i => !recipeMenuItems.Select(r => r.Day).Contains(i));
            var daysToRemove = recipeMenuItems.Where(i => !days.Contains(i.Day));

            foreach (var day in daysToRemove)
            {
                await _client.Document.DeleteDocumentAsync(day._id).ConfigureAwait(false);
            }

            foreach (var day in daysToAdd)
            {
                await _client.Document.PostDocumentAsync("InMenu", new NewInMenuModel { Day = day, _from = "Recipe/" + recipeKey, _to = "User/" + userKey });
            }
        }

        public async Task<IEnumerable<ShoppingListItemModel>> GetShoppingList(string userKey)
        {
            return (await _client.Document.GetDocumentAsync<UserModel>("User", userKey)).ShoppingList;
        }

        public async Task AddToShoppingList(string userKey, ShoppingListItemModel item)
        {
            var user = await _client.Document.GetDocumentAsync<UserModel>("User", userKey);
            user.ShoppingList.Add(item);
            await _client.Document.PutDocumentAsync(user._id, user);
        }

        public async Task RemoveFromShoppingList(string userKey, uint index)
        {
            var user = await _client.Document.GetDocumentAsync<UserModel>("User", userKey);
            user.ShoppingList.RemoveAt((int)index);
            await _client.Document.PutDocumentAsync(user._id, user);
        }
    }
}
