using System.Net;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class DeleteVertexCollectionResponse : GraphResponse
    {
        [JsonConstructor]
        public DeleteVertexCollectionResponse(HttpStatusCode code, GraphResult graph) : base(code, graph)
        {
        }

        public DeleteVertexCollectionResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
