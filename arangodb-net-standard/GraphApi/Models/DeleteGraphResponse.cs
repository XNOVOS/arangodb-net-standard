using System.Net;
using ArangoDBNetStandard.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class DeleteGraphResponse : ResponseBase
    {
        public bool Removed { get; }

        [JsonConstructor]
        public DeleteGraphResponse(bool error, HttpStatusCode code, bool removed) : base(new ApiResponse(error, code))
        {
            Removed = removed;
        }

        public DeleteGraphResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }
    }
}
