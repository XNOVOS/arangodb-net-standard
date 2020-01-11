using System.Collections.Generic;

namespace ArangoDBNetStandard.GraphApi.Models
{
    /// <summary>
    /// Represents query parameters used when creating a new graph.
    /// </summary>
    public class PostGraphQuery : RequestOptionsBase
    {
        /// <summary>
        /// Whether the request should wait until synced to disk.
        /// </summary>
        public bool? WaitForSync { get; set; }

        protected override void PrepareQueryStringValues(IDictionary<string, string> values)
        {
            if (WaitForSync.HasValue)
                values.Add(nameof(WaitForSync).ToCamelCase(), WaitForSync.ToString().ToLowerInvariant());
        }
    }
}
