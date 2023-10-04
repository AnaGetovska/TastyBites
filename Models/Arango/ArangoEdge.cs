namespace TastyBytesReact.Models.Arango
{
    public class ArangoEdge : IArangoEdge
    {
        /// <inheritdoc/>
        public string _key { get; set; }
        /// <inheritdoc/>
        public string _id { get; set; }
        /// <inheritdoc/>
        public string _rev { get; set; }
        /// <inheritdoc/>
        public string _from { get; set; }
        /// <inheritdoc/>
        public string _to { get; set; }
    }
}