using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.CollectionApi.Models
{
    public class PostCollectionResponse : ResponseBase
    {
        public PostCollectionResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }

        [JsonConstructor]
        public PostCollectionResponse(bool error, HttpStatusCode code, bool doCompact, string globallyUniqueId, string id, int indexBuckets, bool isSystem, bool isVolatile, long journalSize, PostCollectionResponseCollectionKeyOptions keyOptions, string name, int status, string statusString, int type, bool waitForSync) : base(new ApiResponse(error, code, null, null))
        {
            DoCompact = doCompact;
            GloballyUniqueId = globallyUniqueId;
            Id = id;
            IndexBuckets = indexBuckets;
            IsSystem = isSystem;
            IsVolatile = isVolatile;
            JournalSize = journalSize;
            KeyOptions = keyOptions;
            Name = name;
            Status = status;
            StatusString = statusString;
            Type = type;
            WaitForSync = waitForSync;
        }

        public bool DoCompact { get; }

        public string GloballyUniqueId { get; }

        public string Id { get; }

        public int IndexBuckets { get; }

        public bool IsSystem { get; }

        public bool IsVolatile { get; }

        public long JournalSize { get; }

        public PostCollectionResponseCollectionKeyOptions KeyOptions { get; }

        public string Name { get; }

        public int Status { get; }

        public string StatusString { get; }

        public int Type { get; }


        public bool WaitForSync { get; }
    }
}