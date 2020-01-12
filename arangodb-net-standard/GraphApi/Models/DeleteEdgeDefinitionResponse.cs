using System.Net;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class DeleteEdgeDefinitionResponse : GraphResponse
    {
        [JsonConstructor]
        public DeleteEdgeDefinitionResponse(HttpStatusCode code, GraphResult graph) : base(code, graph)
        {
        }

        public DeleteEdgeDefinitionResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
