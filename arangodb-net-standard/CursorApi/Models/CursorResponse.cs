using System.Collections.Generic;
using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.CursorApi.Models
{
    /// <summary>
    /// Response from ArangoDB when creating a new cursor.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CursorResponse<T> : ResponseBase
    {
        [JsonConstructor]
        public CursorResponse(bool error, long count, HttpStatusCode code, CursorResponseExtra extra, bool cached,
            bool hasMore, IEnumerable<T> result, string id) : base(new ApiResponse(error, code, null, null))
        {
            Count = count;
            Extra = extra;
            Cached = cached;
            HasMore = hasMore;
            Id = id;
            Results = new List<T>(result).AsReadOnly();
        }

        /// <summary>
        /// the total number of result documents available
        /// (only available if the query was executed with the count attribute set)
        /// </summary>
        public long Count { get; }

        /// <summary>
        /// Optional object with extra information about the query result contained
        /// in its <see cref="CursorResponseExtra.Stats"/> sub-attribute.
        /// For data-modification queries, the sub-attribute will contain the number of
        /// modified documents and the number of documents that could not be modified
        /// due to an error (if ignoreErrors query option is specified).
        /// </summary>
        public CursorResponseExtra Extra { get; }

        /// <summary>
        /// Indicates whether the query result was served from the query cache or not.
        /// If the query result is served from the query cache, the extra return attribute
        /// will not contain any stats sub-attribute and no profile sub-attribute.
        /// </summary>
        public bool Cached { get; }

        /// <summary>
        /// Whether there are more results available for the cursor on the server.
        /// </summary>
        public bool HasMore { get; }

        /// <summary>
        /// Result documents (might be empty if query has no results).
        /// </summary>
        public IReadOnlyList<T> Results { get; }

        /// <summary>
        /// ID of temporary cursor created on the server (optional).
        /// </summary>
        public string Id { get; }

        public CursorResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
