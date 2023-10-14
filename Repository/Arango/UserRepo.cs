using ArangoDBNetStandard;
using TastyBytesReact.Models;
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
    }
}
