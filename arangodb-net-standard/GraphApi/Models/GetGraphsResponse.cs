using System.Collections.Generic;
using System.Net;
using ArangoDBNetStandard.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    /// <summary>
    /// Represents a response containing the list of graph.
    /// </summary>
    [JsonObject]
    public class GetGraphsResponse : ListResponse<GraphResult>
    {
        [JsonConstructor]
        public GetGraphsResponse(HttpStatusCode code, bool error, IEnumerable<GraphResult> graphs) : base(graphs, new ApiResponse(error, code))
        {
            
        }

        /// <summary>
        /// The list of graph.
        /// Note: The <see cref="GraphResult.Name"/> property is null for <see cref="GraphApiClient.GetGraphsAsync"/> in ArangoDB 4.5.2 and below,
        /// in which case you can use <see cref="GraphResult._key"/> instead.
        /// </summary>
        public GetGraphsResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }
    }
}
