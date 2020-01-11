using System.Net;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    /// <summary>
    /// Represents a response containing information about the newly created graph.
    /// </summary>
    public class PostGraphResponse : GraphResponse
    {
        [JsonConstructor]
        public PostGraphResponse(HttpStatusCode code, GraphResult graph) : base(code, graph)
        {
        }

        public PostGraphResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
