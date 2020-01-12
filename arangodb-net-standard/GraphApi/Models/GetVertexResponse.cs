using System.Net;
using ArangoDBNetStandard.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class GetVertexResponse<T> : ResponseBase
    {
        /// <summary>
        /// The complete vertex.
        /// </summary>
        public T Vertex { get; }

        public GetVertexResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }

        [JsonConstructor]
        public GetVertexResponse(bool error, HttpStatusCode code, T vertex) : base(new ApiResponse(error, code))
        {
            Vertex = vertex;
        }
    }
}
