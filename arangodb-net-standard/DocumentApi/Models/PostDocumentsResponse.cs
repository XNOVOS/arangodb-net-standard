using System.Collections;
using System.Collections.Generic;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    /// <summary>
    /// Response after posting multiple documents
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PostDocumentsResponse<T>: ResponseBase, IReadOnlyList<PostDocumentResponse<T>>
    {
        private readonly IList<PostDocumentResponse<T>> _responses;

        public PostDocumentsResponse(IEnumerable<PostDocumentResponse<T>> responses) : base(null)
        {
            _responses = new List<PostDocumentResponse<T>>(responses);
        }
        public PostDocumentsResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }

        public IEnumerator<PostDocumentResponse<T>> GetEnumerator()
        {
            return _responses.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _responses.Count;

        public PostDocumentResponse<T> this[int index] => _responses[index];
    }
}