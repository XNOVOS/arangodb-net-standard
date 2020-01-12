using System.Net;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    /// <summary>
    /// Represents a response containing information about the graph
    /// and its new edge definition.
    /// </summary>
    public class PostEdgeDefinitionResponse : GraphResponse
    {
        [JsonConstructor]
        public PostEdgeDefinitionResponse(HttpStatusCode code, GraphResult graph) : base(code, graph)
        {
        }

        public PostEdgeDefinitionResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
