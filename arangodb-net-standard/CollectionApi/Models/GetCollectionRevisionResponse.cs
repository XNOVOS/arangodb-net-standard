using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.CollectionApi.Models
{
    public class GetCollectionRevisionResponse : ResponseBase
    {
        [JsonConstructor]
        public GetCollectionRevisionResponse(bool error, HttpStatusCode code, bool waitForSync, int journalSize, bool isVolatile, bool isSystem, int indexBuckets, CollectionKeyOptions keyOptions, string globallyUniqueId, string statusString, string id, string revision, int status, int type, string name, bool doCompact) : base(new ApiResponse(error, code, null, null))
        {
            WaitForSync = waitForSync;
            JournalSize = journalSize;
            IsVolatile = isVolatile;
            IsSystem = isSystem;
            IndexBuckets = indexBuckets;
            KeyOptions = keyOptions;
            GloballyUniqueId = globallyUniqueId;
            StatusString = statusString;
            Id = id;
            Revision = revision;
            Status = status;
            Type = type;
            Name = name;
            DoCompact = doCompact;
        }

        public bool WaitForSync { get; }

        public int JournalSize { get; }

        public bool IsVolatile { get; }

        public bool IsSystem { get; }

        public int IndexBuckets { get; }

        public CollectionKeyOptions KeyOptions { get; }

        public string GloballyUniqueId { get; }

        public string StatusString { get; }

        public string Id { get; }

        public string Revision { get; }

        public int Status { get; }

        public int Type { get; }

        public string Name { get; }

        public bool DoCompact { get; }

        public GetCollectionRevisionResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
