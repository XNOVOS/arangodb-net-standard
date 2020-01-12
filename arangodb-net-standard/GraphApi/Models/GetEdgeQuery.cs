using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.GraphApi.Models
{
    /// <summary>
    /// Represents query parameters used when fetching an edge in a graph.
    /// </summary>
    public class GetEdgeQuery : RequestOptionsBase
    {
        /// <summary>
        /// Can contain a revision.
        /// If this is set, a document is only returned if it has exactly this revision.
        /// </summary>
        public string Rev { get; set; }
    }
}
