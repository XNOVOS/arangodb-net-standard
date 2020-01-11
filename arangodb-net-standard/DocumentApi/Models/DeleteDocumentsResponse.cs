using System.Collections;
using System.Collections.Generic;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    public class DeleteDocumentsResponse<T>: ResponseBase, IReadOnlyList<DeleteDocumentResponse<T>>
    {
        private readonly IList<DeleteDocumentResponse<T>> _responses;

        public DeleteDocumentsResponse(IEnumerable<DeleteDocumentResponse<T>> responses) : base(null)
        {
            _responses = new List<DeleteDocumentResponse<T>>(responses);
        }
        public DeleteDocumentsResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }

        public IEnumerator<DeleteDocumentResponse<T>> GetEnumerator()
        {
            return _responses.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _responses.Count;

        public DeleteDocumentResponse<T> this[int index] => _responses[index];
    }
}