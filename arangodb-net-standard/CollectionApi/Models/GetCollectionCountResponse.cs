using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.CollectionApi.Models
{
    public class GetCollectionCountResponse : ResponseBase
    {
        [JsonConstructor]
        public GetCollectionCountResponse(bool error, HttpStatusCode code, bool cacheEnabled, CollectionKeyOptions keyOptions, int count, bool isSystem, string globallyUniqueId, string id, string name, int status, string statusString, int type, bool waitForSync) : base(new ApiResponse(error, code, null, null))
        {
            CacheEnabled = cacheEnabled;
            KeyOptions = keyOptions;
            Count = count;
            IsSystem = isSystem;
            GloballyUniqueId = globallyUniqueId;
            Id = id;
            Name = name;
            Status = status;
            StatusString = statusString;
            Type = type;
            WaitForSync = waitForSync;
        }

        public bool CacheEnabled { get; }

        public CollectionKeyOptions KeyOptions { get; }

        public int Count { get; }

        public bool IsSystem { get; }

        public string GloballyUniqueId { get; }

        public string Id { get; }

        public string Name { get; }

        public int Status { get; }

        public string StatusString { get; }

        public int Type { get; }

        public bool WaitForSync { get; }

        public GetCollectionCountResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
