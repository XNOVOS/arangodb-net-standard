using System.Collections.Generic;
using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.CursorApi.Models
{
    public class PutCursorResponse<T> : ResponseBase
    {
        public string Id { get; }

        public IReadOnlyList<T> Results { get; }

        public bool HasMore { get; }

        public long Count { get; }

        public PutCursorResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }

        [JsonConstructor]
        public PutCursorResponse(string id, IEnumerable<T> result, bool hasMore, long count, bool error, HttpStatusCode code) : base(new ApiResponse(error, code, null, null))
        {
            Id = id;
            Results = new List<T>(result).AsReadOnly();
            HasMore = hasMore;
            Count = count;
        }
    }
}