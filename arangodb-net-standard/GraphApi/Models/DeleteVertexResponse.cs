using System.Net;
using ArangoDBNetStandard.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class DeleteVertexResponse<T> : ResponseBase
    {
        public T Old { get; }

        public bool Removed { get; }

        [JsonConstructor]
        public DeleteVertexResponse(HttpStatusCode code, bool error, T old, bool removed) : base(new ApiResponse(error, code))
        {
            Old = old;
            Removed = removed;
        }

        public DeleteVertexResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }
    }
}
