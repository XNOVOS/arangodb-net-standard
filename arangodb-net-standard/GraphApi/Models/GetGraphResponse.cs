using System.Net;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class GetGraphResponse : GraphResponse
    {
        [JsonConstructor]
        public GetGraphResponse(HttpStatusCode code, GraphResult graph) : base(code, graph)
        {
        }

        public GetGraphResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
