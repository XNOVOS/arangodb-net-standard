using System.Net;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public abstract class GraphResponse : ResponseBase
    {
        [JsonConstructor]
        public GraphResponse(HttpStatusCode code, GraphResult graph) : base(new ApiResponse(false, code, null, null))
        {
            Graph = graph;
        }

        public GraphResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }

        public GraphResult Graph { get; }
    }
}