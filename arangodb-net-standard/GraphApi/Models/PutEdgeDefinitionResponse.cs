using System.Net;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class PutEdgeDefinitionResponse : GraphResponse
    {
        [JsonConstructor]
        public PutEdgeDefinitionResponse(HttpStatusCode code, GraphResult graph) : base(code, graph)
        {
        }

        public PutEdgeDefinitionResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
