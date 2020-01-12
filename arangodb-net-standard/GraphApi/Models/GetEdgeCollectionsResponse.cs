using System.Collections.Generic;
using System.Net;
using ArangoDBNetStandard.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    /// <summary>
    /// Represents a response containing the list of vertex collections within a graph.
    /// </summary>
    [JsonObject]
    public class GetEdgeCollectionsResponse : ListResponse<string>
    {
        [JsonConstructor]
        public GetEdgeCollectionsResponse(bool error, HttpStatusCode code, int? errorNum, IEnumerable<string> collections) : base(collections, new ApiResponse(error, code, null, errorNum))
        {
        }

        public GetEdgeCollectionsResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }
    }
}
