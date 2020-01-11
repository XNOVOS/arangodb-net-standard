using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.CollectionApi.Models
{
    public class GetCollectionPropertiesResponse : ResponseBase
    {
        [JsonConstructor]
        public GetCollectionPropertiesResponse(bool waitForSync, bool doCompact, int journalSize, CollectionKeyOptions keyOptions, bool isVolatile, int? numberOfShards, string shardKeys, int? replicationFactor, string shardingStrategy, bool error, HttpStatusCode code, bool cacheEnabled, bool isSystem, string globallyUniqueId, string objectId, string id, string name, int status, string statusString, int type) : base(new ApiResponse(error, code, null, null))
        {
            WaitForSync = waitForSync;
            DoCompact = doCompact;
            JournalSize = journalSize;
            KeyOptions = keyOptions;
            IsVolatile = isVolatile;
            NumberOfShards = numberOfShards;
            ShardKeys = shardKeys;
            ReplicationFactor = replicationFactor;
            ShardingStrategy = shardingStrategy;
            CacheEnabled = cacheEnabled;
            IsSystem = isSystem;
            GloballyUniqueId = globallyUniqueId;
            ObjectId = objectId;
            Id = id;
            Name = name;
            Status = status;
            StatusString = statusString;
            Type = type;
        }

        public bool WaitForSync { get; }
        public bool DoCompact { get; }
        public int JournalSize { get; }
        public CollectionKeyOptions KeyOptions { get; }
        public bool IsVolatile { get; }
        public int? NumberOfShards { get; }
        public string ShardKeys { get; }
        public int? ReplicationFactor { get; }
        public string ShardingStrategy { get; }

        public bool CacheEnabled { get; }
        public bool IsSystem { get; }
        public string GloballyUniqueId { get; }
        public string ObjectId { get; }
        public string Id { get; }
        public string Name { get; }
        public int Status { get; }
        public string StatusString { get; }
        public int Type { get; }

        public GetCollectionPropertiesResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
