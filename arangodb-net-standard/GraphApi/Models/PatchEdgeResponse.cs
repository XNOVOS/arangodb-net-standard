using System.Net;
using ArangoDBNetStandard.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class PatchEdgeResponse<T> : ResponseBase
    {
        public T New { get; }

        public T Old { get; }

        public PatchEdgeResult Edge { get; }

        [JsonConstructor]
        public PatchEdgeResponse(HttpStatusCode code, bool error, T @new, T old, PatchEdgeResult edge) : base(new ApiResponse(error, code))
        {
            New = @new;
            Old = old;
            Edge = edge;
        }

        public PatchEdgeResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }
    }
}
