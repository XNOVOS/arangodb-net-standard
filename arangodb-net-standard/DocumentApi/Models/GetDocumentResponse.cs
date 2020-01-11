using Newtonsoft.Json;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    public class GetDocumentResponse<T> : ResponseBase
    {
        public T Document { get; }

        [JsonConstructor]
        public GetDocumentResponse(T document) : base(null)
        {
            Document = document;
        }
        public GetDocumentResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}