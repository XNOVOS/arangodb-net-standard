using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.CollectionApi.Models
{
    public class RenameCollectionResponse : ResponseBase
    {
        [JsonConstructor]
        public RenameCollectionResponse(string id, string name, int status, int type, bool isSystem, HttpStatusCode code, bool error) : base(new ApiResponse(error, code, null, null))
        {
            Id = id;
            Name = name;
            Status = status;
            Type = type;
            IsSystem = isSystem;
        }

        public string Id { get; }

        public string Name { get; }

        public int Status { get; }

        public int Type { get; }

        public bool IsSystem { get; }

        public RenameCollectionResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
