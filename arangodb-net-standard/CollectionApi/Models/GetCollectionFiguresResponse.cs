using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.CollectionApi.Models
{
    public class GetCollectionFiguresResponse : ResponseBase
    {
        [JsonConstructor]
        public GetCollectionFiguresResponse(FiguresResult figures, CollectionKeyOptions keyOptions, string globallyUniqueId, string statusString, string id, int indexBuckets, bool error, HttpStatusCode code, int type, int status, int journalSize, bool isVolatile, string name, bool doCompact, bool isSystem, int count, bool waitForSync) : base(new ApiResponse(error, code, null, null))
        {
            Figures = figures;
            KeyOptions = keyOptions;
            GloballyUniqueId = globallyUniqueId;
            StatusString = statusString;
            Id = id;
            IndexBuckets = indexBuckets;
            Type = type;
            Status = status;
            JournalSize = journalSize;
            IsVolatile = isVolatile;
            Name = name;
            DoCompact = doCompact;
            IsSystem = isSystem;
            Count = count;
            WaitForSync = waitForSync;
        }

        public FiguresResult Figures { get; }

        public CollectionKeyOptions KeyOptions { get; }

        public string GloballyUniqueId { get; }

        public string StatusString { get; }

        public string Id { get; }

        public int IndexBuckets { get; }

        public int Type { get; }

        public int Status { get; }

        public int JournalSize { get; }

        public bool IsVolatile { get; }

        public string Name { get; }

        public bool DoCompact { get; }

        public bool IsSystem { get; }

        public int Count { get; }

        public bool WaitForSync { get; }

        public GetCollectionFiguresResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
