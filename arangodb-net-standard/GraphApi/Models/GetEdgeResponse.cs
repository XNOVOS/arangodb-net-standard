using System.Net;
using ArangoDBNetStandard.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    /// <summary>
    /// Represents a response containing an edge in a graph.
    /// </summary>
    /// <typeparam name="T">The type of the edge document.</typeparam>
    public class GetEdgeResponse<T> : ResponseBase
    {
        /// <summary>
        /// The complete edge.
        /// </summary>
        public T Edge { get; }

        public GetEdgeResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }

        [JsonConstructor]
        public GetEdgeResponse(bool error, HttpStatusCode code, T edge) : base(new ApiResponse(error, code))
        {
            Edge = edge;
        }
    }
}
