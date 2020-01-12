using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.CollectionApi.Models
{
    public class PutCollectionPropertyResponse : ResponseBase
    {
        [JsonConstructor]
        public PutCollectionPropertyResponse(string id, string name, bool waitForSync, long journalSize, int status, int type, bool isSystem, bool isVolatile, bool doCompact, CollectionKeyOptions keyOptions, string globallyUniqueId, bool error, HttpStatusCode code, string statusString, int indexBuckets) : base(new ApiResponse(error, code, null, null))
        {
            Id = id;
            Name = name;
            WaitForSync = waitForSync;
            JournalSize = journalSize;
            Status = status;
            Type = type;
            IsSystem = isSystem;
            IsVolatile = isVolatile;
            DoCompact = doCompact;
            KeyOptions = keyOptions;
            GloballyUniqueId = globallyUniqueId;
            StatusString = statusString;
            IndexBuckets = indexBuckets;
        }

        public string Id { get; }

        public string Name { get; }

        public bool WaitForSync { get; }

        public long JournalSize { get; }

        public int Status { get; }

        public int Type { get; }

        public bool IsSystem { get; }

        public bool IsVolatile { get; }

        public bool DoCompact { get; }

        public CollectionKeyOptions KeyOptions { get; }

        public string GloballyUniqueId { get; }

        public string StatusString { get; }

        public int IndexBuckets { get; }

        public PutCollectionPropertyResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
