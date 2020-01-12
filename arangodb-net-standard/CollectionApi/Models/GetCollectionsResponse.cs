using System.Collections;
using System.Collections.Generic;
using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.CollectionApi.Models
{
    [JsonObject]
    public class GetCollectionsResponse : ResponseBase,  IReadOnlyList<GetCollectionResponse>
    {
        private readonly IReadOnlyList<GetCollectionResponse> _results;

        [JsonConstructor]
        public GetCollectionsResponse(bool error, HttpStatusCode code, IEnumerable<GetCollectionResponse> result) : base(new ApiResponse(error, code, null, null))
        {
            _results = new List<GetCollectionResponse>(result).AsReadOnly();
        }

        public GetCollectionsResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }

        public IEnumerator<GetCollectionResponse> GetEnumerator()
        {
            return _results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _results.Count;

        public GetCollectionResponse this[int index] => _results[index];
    }
}
