using System.Net;
using ArangoDBNetStandard.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class PutVertexResponse<T> : ResponseBase
    {
        public T New { get; }

        public T Old { get; }

        public PutVertexResult Vertex { get; }

        [JsonConstructor]
        public PutVertexResponse(T @new, T old, HttpStatusCode code, PutVertexResult vertex, bool error) : base(new ApiResponse(error, code))
        {
            New = @new;
            Old = old;
            Vertex = vertex;
        }

        public PutVertexResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }
    }
}
