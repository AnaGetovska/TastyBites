using TastyBytesReact.Models.Arango;

namespace TastyBytesReact.Models.Edges
{
    public class InMenuModel : ArangoEdge
    {
        /// <summary>
        /// Day of the week.
        /// </summary>
        public DayEnum Day { get; set; }
    }
}
