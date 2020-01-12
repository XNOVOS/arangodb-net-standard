using System.Collections.Generic;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.GraphApi.Models
{
    /// <summary>
    /// Represents query parameters used when creating a new graph edge.
    /// </summary>
    public class PostEdgeQuery : RequestOptionsBase
    {
        /// <summary>
        /// Whether the response should contain the complete new version of the document.
        /// </summary>
        public bool? ReturnNew { get; set; }

        /// <summary>
        /// Whether the request should wait until synced to disk.
        /// </summary>
        public bool? WaitForSync { get; set; }
    }
}
