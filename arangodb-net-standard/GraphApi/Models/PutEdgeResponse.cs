using System.Net;
using ArangoDBNetStandard.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class PutEdgeResponse<T> : ResponseBase
    {
        public T Old { get; }

        public T New { get; }

        public PutEdgeResult Edge { get; }

        [JsonConstructor]
        public PutEdgeResponse(bool error, HttpStatusCode code, T old, T @new, PutEdgeResult edge) : base(new ApiResponse(error, code))
        {
            Old = old;
            New = @new;
            Edge = edge;
        }

        public PutEdgeResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }
    }
}
