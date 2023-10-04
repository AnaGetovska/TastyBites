namespace TastyBytesReact.Models.Arango
{
    public interface IArangoEdge : IArangoDocument
    {
        /// <summary>
        /// Arango document identifier in the form of {Collection}/<see cref="_key"/>.
        /// </summary>
        string _id { get; set; }

        /// <summary>
        /// Arango document key.
        /// </summary>
        string _key { get; set; }

        /// <summary>
        /// Arango revision hash.
        /// </summary>
        string _rev { get; set; }

        /// <summary>
        /// Incoming relation of the Arango document id.
        /// </summary>
        string _from { get; set; }

        /// <summary>
        /// Outgoing relation of the Arango document id.
        /// </summary>
        string _to { get; set; }
    }
}
//Category/4470 from Category/4498