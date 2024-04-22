using TastyBytesReact.Models.Arango;

namespace TastyBytesReact.Models.Nodes
{
    /// <summary>
    /// Model representation of the database Category documents
    /// describing each dish or ingredient category.
    /// </summary>
    public class ImageModel : ArangoDocument
    {
        /// <summary>
        /// Image body
        /// </summary>
        public required string Body { get; set; }

        /// <summary>
        /// Last modified date by UNIX timestamp.
        /// </summary>
        public string LastModified { get; set; }
    }
}
