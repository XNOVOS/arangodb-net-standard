using System.Net;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    /// <summary>
    /// Represents a response containing information about the modified graph.
    /// </summary>
    public class PostVertexCollectionResponse : GraphResponse
    {
        [JsonConstructor]
        public PostVertexCollectionResponse(HttpStatusCode code, GraphResult graph) : base(code, graph)
        {
        }

        public PostVertexCollectionResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
