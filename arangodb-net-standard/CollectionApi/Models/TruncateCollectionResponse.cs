using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.CollectionApi.Models
{
    public class TruncateCollectionResponse : ResponseBase
    {
        public TruncateCollectionResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }

        [JsonConstructor]
        public TruncateCollectionResponse(bool error, HttpStatusCode code, int status, string name, int type, bool isSystem, string globallyUniqueId, string id) : base(new ApiResponse(error, code, null, null))
        {
            Status = status;
            Name = name;
            Type = type;
            IsSystem = isSystem;
            GloballyUniqueId = globallyUniqueId;
            Id = id;
        }

        public int Status { get; }

        public string Name { get; }

        public int Type { get; }

        public bool IsSystem { get; }

        public string GloballyUniqueId { get; }

        public string Id { get; }
    }
}