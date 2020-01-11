using System.Collections;
using System.Collections.Generic;
using System.Net;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    /// <summary>
    /// Represents a response containing the list of graph.
    /// </summary>
    [JsonObject]
    public class GetGraphsResponse : ResponseBase, IReadOnlyList<GraphResult>
    {
        private readonly IList<GraphResult> _results;

        [JsonConstructor]
        public GetGraphsResponse(HttpStatusCode code, bool error, IEnumerable<GraphResult> graphs) : base(new ApiResponse(error, code))
        {
            _results = new List<GraphResult>(graphs ?? new List<GraphResult>());
        }

        /// <summary>
        /// The list of graph.
        /// Note: The <see cref="GraphResult.Name"/> property is null for <see cref="GraphApiClient.GetGraphsAsync"/> in ArangoDB 4.5.2 and below,
        /// in which case you can use <see cref="GraphResult._key"/> instead.
        /// </summary>
        public GetGraphsResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }

        public IEnumerator<GraphResult> GetEnumerator()
        {
            return _results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _results.Count;

        public GraphResult this[int index] => _results[index];
    }
}
