using Newtonsoft.Json;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    public class DeleteDocumentResponse<T>: ResponseBaseWithDocumentProperties
    {
        public T Old { get; }

        [JsonConstructor]
        public DeleteDocumentResponse(string _key, string _id, string _rev, T Old) : base(_key, _id, _rev)
        {
            this.Old = Old;
        }

        public DeleteDocumentResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}