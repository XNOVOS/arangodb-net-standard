using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.CollectionApi.Models
{
    public class GetCollectionResponse : ResponseBase
    {
        public GetCollectionResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }

        [JsonConstructor]
        public GetCollectionResponse(bool? error, HttpStatusCode? code, int type, bool isSystem, string globallyUniqueId, string id, string name, int status) : base(error == null ? null : new ApiResponse(error, code, null, null))
        {
            Type = type;
            IsSystem = isSystem;
            GloballyUniqueId = globallyUniqueId;
            Id = id;
            Name = name;
            Status = status;
        }

        public int Type { get; }

        public bool IsSystem { get; }

        public string GloballyUniqueId { get; }

        public string Id { get; }

        public string Name { get; }

        public int Status { get; }
    }
}
