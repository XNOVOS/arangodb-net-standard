using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.CollectionApi.Models
{
    public class DeleteCollectionResponse : ResponseBase
    {
        public string Id { get; }

        [JsonConstructor]
        public DeleteCollectionResponse(bool error, HttpStatusCode code, string id) : base(new ApiResponse(error, code, null, null))
        {
            Id = id;
        }

        public DeleteCollectionResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}