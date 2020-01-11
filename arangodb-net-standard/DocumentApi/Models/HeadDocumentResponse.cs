using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    public class HeadDocumentResponse : ResponseBase
    {
        [JsonConstructor]
        public HeadDocumentResponse(HttpStatusCode code, EntityTagHeaderValue etag) : base(null)
        {
            Code = code;
            Etag = etag;
        }

        public HttpStatusCode Code { get; }

        public EntityTagHeaderValue Etag { get; }

        public HeadDocumentResponse(EntityTagHeaderValue etag, ApiResponse errorDetails) : base(errorDetails)
        {
            Code = errorDetails.Code;
            Etag = etag;
        }
    }
}
