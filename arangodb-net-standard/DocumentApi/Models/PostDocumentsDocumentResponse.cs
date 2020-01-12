using System.Net;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    /// <summary>
    /// Response model for a single POST Document request.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PostDocumentsDocumentResponse<T> : PostDocumentResponse<T>
    {
        public PostDocumentsDocumentResponse(string _key, string _id, string _rev, ApiResponse errorDetails = null) : base(_key, _id, _rev, default, default)
        {
        }
        public bool Error { get; set; }

        public string ErrorMessage { get; set; }

        public int ErrorNum { get; set; }

        public HttpStatusCode Code { get; set; }
    }
}