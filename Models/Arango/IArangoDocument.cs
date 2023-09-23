namespace TastyBytesReact.Models.Arango
{
    public interface IArangoDocument
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
    }
}
