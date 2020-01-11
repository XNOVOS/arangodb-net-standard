using Newtonsoft.Json;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    /// <summary>
    /// Response after posting a single document
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PostDocumentResponse<T> : ResponseBaseWithDocumentProperties
    {
        public PostDocumentResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }

        [JsonConstructor]
        public PostDocumentResponse(string _key, string _id, string _rev, T New, T Old) : base(_key, _id, _rev)
        {
            this.New = New;
            this.Old = Old;
        }

        /// <summary>
        /// Deserialized copy of the new document object. This will only be present if requested with the
        /// <see cref="PostDocumentsQuery.ReturnNew"/> option.
        /// </summary>
        public T New { get; set; }

        /// <summary>
        /// Deserialized copy of the old document object. This will only be present if requested with the
        /// <see cref="PostDocumentsQuery.ReturnOld"/> option.
        /// </summary>
        public T Old { get; set; }
    }

}