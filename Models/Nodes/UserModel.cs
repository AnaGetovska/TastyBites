using TastyBytesReact.Models.Arango;

namespace TastyBytesReact.Models.Nodes
{
    public class UserModel : ArangoDocument
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public IList<ShoppingListItemModel> ShoppingList { get; set; }
    }
}
