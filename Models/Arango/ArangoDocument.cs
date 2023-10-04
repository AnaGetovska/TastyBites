namespace TastyBytesReact.Models.Arango
{
    public class ArangoDocument : IArangoDocument
    {
        /// <inheritdoc/>
        public string _id { get; set; }
        /// <inheritdoc/>
        public string _key { get; set; }
        /// <inheritdoc/>
        public string _rev { get; set; }
    }
}
