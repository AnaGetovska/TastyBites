using ArangoDBNetStandard;
using TastyBytesReact.Models;
using TastyBytesReact.Models.Arango;
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

        public async Task<IEnumerable<string>> GetFavRecipeIds(string userKey)
        {
            var res = await _client.Cursor.PostCursorAsync<string>(
              @"FOR v, e IN 1 INBOUND CONCAT(""User/"",@userKey) LikedBy RETURN e._from", new Dictionary<string, object>() { { "userKey", userKey } })
              .ConfigureAwait(false);
            return res.Result;
        }

        public async Task RemoveFromFavourites(string userKey, string recipeKey)
        {
            await _client.Cursor.PostCursorAsync<ArangoEdge>(
               @"FOR doc IN User FILTER doc._id == CONCAT(""User/"",@userKey)
                    FOR v, e IN 1 INBOUND doc._id LikedBy
                    FILTER e._from == CONCAT(""Recipe/"",@recipeKey)
                REMOVE e IN LikedBy", new Dictionary<string, object>() { { "userKey", userKey }, { "recipeKey", recipeKey } })
               .ConfigureAwait(false);
        }

        public async Task AddToFavourites(string userKey, string recipeKey)
        {
            var likedBy = new ArangoEdge()
            {
                _from = "Recipe/" + recipeKey,
                _to = "User/" + userKey
            };

            await _client.Document.PostDocumentAsync<ArangoEdge>("LikedBy", likedBy);
        }
    }
}
