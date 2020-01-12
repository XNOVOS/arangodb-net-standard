using System.Collections;
using System.Collections.Generic;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    public class PatchDocumentsResponse<T> : ResponseBase, IReadOnlyList<PatchDocumentResponse<T>>
    {
        private readonly IList<PatchDocumentResponse<T>> _responses;

        public PatchDocumentsResponse(IEnumerable<PatchDocumentResponse<T>> responses) : base(null)
        {
            _responses = new List<PatchDocumentResponse<T>>(responses);
        }

        public PatchDocumentsResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }

        public IEnumerator<PatchDocumentResponse<T>> GetEnumerator()
        {
            return _responses.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _responses.Count;

        public PatchDocumentResponse<T> this[int index] => _responses[index];
    }
}
