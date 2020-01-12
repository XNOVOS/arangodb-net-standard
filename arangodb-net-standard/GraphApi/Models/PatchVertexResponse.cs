using System.Net;
using ArangoDBNetStandard.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class PatchVertexResponse<T> : ResponseBase
    {
        public T New { get; }

        public T Old { get; }

        public PatchVertexResult Vertex { get; }

        [JsonConstructor]
        public PatchVertexResponse(T @new, T old, HttpStatusCode code, bool error, PatchVertexResult vertex) : base(new ApiResponse(error, code))
        {
            New = @new;
            Old = old;
            Vertex = vertex;
        }

        public PatchVertexResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }
    }
}
