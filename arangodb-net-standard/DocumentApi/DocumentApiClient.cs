using ArangoDBNetStandard.DocumentApi.Models;
using ArangoDBNetStandard.Serialization;
using ArangoDBNetStandard.Transport;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.DocumentApi
{
    /// <summary>
    /// Provides access to ArangoDB document API.
    /// </summary>
    public class DocumentApiClient : ApiClientBase, IDocumentApiClient
    {
        protected override string ApiRootPath => "_api/document";

        /// <summary>
        /// Creates an instance of <see cref="DocumentApiClient"/>
        /// using the provided transport layer and the default JSON serialization.
        /// </summary>
        /// <param name="client"></param>
        public DocumentApiClient(IApiClientTransport transport)
            : base(transport, new JsonNetApiClientSerialization())
        {
            
        }

        /// <summary>
        /// Creates an instance of <see cref="DocumentApiClient"/>
        /// using the provided transport and serialization layers.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="serializer"></param>
        public DocumentApiClient(IApiClientTransport transport, IApiClientSerialization serializer)
            : base(transport, serializer)
        {
            
        }

        /// <summary>
        /// Post a single document.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="document"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PostDocumentResponse<T>> PostDocumentAsync<T>(string collectionName, T document,
            PostDocumentsOptions query = null, CancellationToken cancellationToken = default)
        {
            if (query != null && query.ContentSerializationOptions == null)
            {
                query.ContentSerializationOptions = new ContentSerializationOptions(false, true);
            }

            return await PostRequestAsync($"{ApiRootPath}/{WebUtility.UrlEncode(collectionName)}",
                response => new PostDocumentResponse<T>(response), document,
                query ?? new PostDocumentsOptions
                    {ContentSerializationOptions = new ContentSerializationOptions(false, true)}, cancellationToken);
        }

        /// <summary>
        /// Post multiple documents in a single request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="documents"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PostDocumentsResponse<T>> PostDocumentsAsync<T>(string collectionName,
            IEnumerable<T> documents, PostDocumentsOptions query = null, CancellationToken cancellationToken = default)
        {
            return await PostRequestAsync($"{ApiRootPath}/{WebUtility.UrlEncode(collectionName)}",
                response => new PostDocumentsResponse<T>(response), documents, query, cancellationToken);
        }

        /// <summary>
        /// Replace multiple documents.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="documents"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PostDocumentsResponse<T>> PutDocumentsAsync<T>(string collectionName,
            IEnumerable<T> documents, PutDocumentsOptions query = null, CancellationToken cancellationToken = default)
        {
            return await PutRequestAsync($"{ApiRootPath}/{WebUtility.UrlEncode(collectionName)}",
                response => new PostDocumentsResponse<T>(response), documents, query, cancellationToken);
        }

        /// <summary>
        /// Replaces the document with handle <document-handle> with the one in
        /// the body, provided there is such a document and no precondition is
        /// violated.
        /// PUT/_api/document/{document-handle}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentId"></param>
        /// <param name="doc"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public async Task<PostDocumentResponse<T>> PutDocumentAsync<T>(string documentId, T document, PutDocumentsOptions query = null, CancellationToken cancellationToken = default)
        {
            ValidateDocumentId(documentId);
            return await PutRequestAsync($"{ApiRootPath}/{documentId}",
                response => new PostDocumentResponse<T>(response), document, query, cancellationToken);
        }

        /// <summary>
        /// Get an existing document.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="documentKey"></param>
        /// <returns></returns>
        public async Task<GetDocumentResponse<T>> GetDocumentAsync<T>(string collectionName, string documentKey,
            CancellationToken cancellationToken = default)
        {
            return await GetDocumentAsync<T>(
                $"{WebUtility.UrlEncode(collectionName)}/{WebUtility.UrlEncode(documentKey)}", cancellationToken);
        }

        /// <summary>
        /// Get an existing document based on its Document ID.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public async Task<GetDocumentResponse<T>> GetDocumentAsync<T>(string documentId, CancellationToken cancellationToken = default)
        {
            ValidateDocumentId(documentId);
            return await GetRequestAsync($"{ApiRootPath}/{documentId}",
                response => new GetDocumentResponse<T>(response), null, cancellationToken);
        }

        /// <summary>
        /// Delete a document.
        /// </summary>
        /// <remarks>
        /// This method overload is provided as a convenience when the client does not care about the type of <see cref="DeleteDocumentResponse{T}.Old"/>
        /// in the returned <see cref="DeleteDocumentResponse{object}"/>. Its value will be <see cref="null"/> when 
        /// <see cref="DeleteDocumentsOptions.ReturnOld"/> is either <see cref="false"/> or not set, so this overload is useful in the default case 
        /// when deleting documents.
        /// </remarks>
        /// <param name="collectionName"></param>
        /// <param name="documentKey"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<DeleteDocumentResponse<object>> DeleteDocumentAsync(string collectionName, string documentKey,
            DeleteDocumentsOptions query = null, CancellationToken cancellationToken = default)
        {
            return await DeleteDocumentAsync<object>(
                $"{WebUtility.UrlEncode(collectionName)}/{WebUtility.UrlEncode(documentKey)}", query, cancellationToken);
        }

        /// <summary>
        /// Delete a document based on its document ID.
        /// </summary>
        /// <remarks>
        /// This method overload is provided as a convenience when the client does not care about the type of <see cref="DeleteDocumentResponse{T}.Old"/>
        /// in the returned <see cref="DeleteDocumentResponse{object}"/>. Its value will be <see cref="null"/> when 
        /// <see cref="DeleteDocumentsOptions.ReturnOld"/> is either <see cref="false"/> or not set, so this overload is useful in the default case 
        /// when deleting documents.
        /// </remarks>
        /// <param name="documentId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<DeleteDocumentResponse<object>> DeleteDocumentAsync(string documentId,
            DeleteDocumentsOptions query = null, CancellationToken cancellationToken = default)
        {
            return await DeleteDocumentAsync<object>(documentId, query, cancellationToken);
        }

        /// <summary>
        /// Delete multiple documents based on the passed document selectors.
        /// A document selector is either the document ID or the document Key.
        /// </summary>
        /// <remarks>
        /// This method overload is provided as a convenience when the client does not care about the type of <see cref="DeleteDocumentResponse{T}.Old"/>
        /// in the returned <see cref="DeleteDocumentsResponse{object}"/>. These will be <see cref="null"/> when 
        /// <see cref="DeleteDocumentsOptions.ReturnOld"/> is either <see cref="false"/> or not set, so this overload is useful in the default case 
        /// when deleting documents.
        /// </remarks>
        /// <param name="collectionName"></param>
        /// <param name="selectors"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<DeleteDocumentsResponse<object>> DeleteDocumentsAsync(string collectionName,
            IEnumerable<string> selectors, DeleteDocumentsOptions query = null, CancellationToken cancellationToken = default)
        {
            return await DeleteDocumentsAsync<object>(collectionName, selectors, query, cancellationToken);
        }

        /// <summary>
        /// Delete a document.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="documentKey"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<DeleteDocumentResponse<T>> DeleteDocumentAsync<T>(string collectionName, string documentKey, DeleteDocumentsOptions query = null, CancellationToken cancellationToken = default)
        {
            return await DeleteDocumentAsync<T>(
                $"{WebUtility.UrlEncode(collectionName)}/{WebUtility.UrlEncode(documentKey)}", query,
                cancellationToken);
        }

        /// <summary>
        /// Delete a document based on its document ID.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<DeleteDocumentResponse<T>> DeleteDocumentAsync<T>(string documentId,
            DeleteDocumentsOptions query = null, CancellationToken cancellationToken = default)
        {
            ValidateDocumentId(documentId);
            return await DeleteRequestAsync($"{ApiRootPath}/{documentId}",
                response => new DeleteDocumentResponse<T>(response), query, cancellationToken);
        }

        /// <summary>
        /// Delete multiple documents based on the passed document selectors.
        /// A document selector is either the document ID or the document Key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="selectors"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<DeleteDocumentsResponse<T>> DeleteDocumentsAsync<T>(string collectionName,
            IEnumerable<string> selectors, DeleteDocumentsOptions query = null, CancellationToken cancellationToken = default)
        {
            return await DeleteRequestAsync($"{ApiRootPath}/{WebUtility.UrlEncode(collectionName)}",
                response => new DeleteDocumentsResponse<T>(response), query, selectors, cancellationToken);
        }

        /// <summary>
        /// Partially updates documents, the documents to update are specified
        /// by the _key attributes in the body objects.The body of the
        /// request must contain a JSON array of document updates with the
        /// attributes to patch(the patch documents). All attributes from the
        /// patch documents will be added to the existing documents if they do
        /// not yet exist, and overwritten in the existing documents if they do
        /// exist there.
        /// Setting an attribute value to null in the patch documents will cause a
        /// value of null to be saved for the attribute by default.
        /// If ignoreRevs is false and there is a _rev attribute in a
        /// document in the body and its value does not match the revision of
        /// the corresponding document in the database, the precondition is
        /// violated.
        /// PATCH/_api/document/{collection}
        /// </summary>
        /// <typeparam name="T">Type of the patch object used to partially update documents.</typeparam>
        /// <typeparam name="TResponse">Type of the returned documents, only applies when
        /// <see cref="PatchDocumentsOptions.ReturnNew"/> or <see cref="PatchDocumentsOptions.ReturnOld"/>
        /// are used.</typeparam>
        /// <param name="collectionName"></param>
        /// <param name="patches"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PatchDocumentsResponse<TResponse>> PatchDocumentsAsync<TPatch, TResponse>(
            string collectionName,
            IEnumerable<TPatch> patches,
            PatchDocumentsOptions query = null,
            CancellationToken cancellationToken = default)
        {
            return await PatchRequestAsync(ApiRootPath + "/" + WebUtility.UrlEncode(collectionName),
                response => new PatchDocumentsResponse<TResponse>(response), patches, query, cancellationToken);
        }

        /// <summary>
        /// Partially updates the document identified by document-handle.
        /// The body of the request must contain a JSON document with the
        /// attributes to patch(the patch document). All attributes from the
        /// patch document will be added to the existing document if they do not
        /// yet exist, and overwritten in the existing document if they do exist
        /// there.
        /// PATCH/_api/document/{document-handle}
        /// </summary>
        /// <typeparam name="T">Type of the patch object used to partially update a document.</typeparam>
        /// <typeparam name="TResponse">Type of the returned document, only applies when
        /// <see cref="PatchDocumentOptions.ReturnNew"/> or <see cref="PatchDocumentOptions.ReturnOld"/>
        /// are used.</typeparam>
        /// <param name="collectionName"></param>
        /// <param name="documentKey"></param>
        /// <param name="body"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PatchDocumentResponse<TResponse>> PatchDocumentAsync<TPatch, TResponse>(
            string collectionName,
            string documentKey,
            TPatch body,
            PatchDocumentOptions query = null,
            CancellationToken cancellationToken = default)
        {
            string documentHandle = WebUtility.UrlEncode(collectionName) +
                "/" + WebUtility.UrlEncode(documentKey);

            return await PatchDocumentAsync<TPatch, TResponse>(documentHandle, body, query, cancellationToken);
        }

        /// <summary>
        /// Partially updates the document identified by document-handle.
        /// The body of the request must contain a JSON document with the
        /// attributes to patch(the patch document). All attributes from the
        /// patch document will be added to the existing document if they do not
        /// yet exist, and overwritten in the existing document if they do exist
        /// there.
        /// PATCH/_api/document/{document-handle}
        /// </summary>
        /// <typeparam name="TPatch">Type of the patch object used to partially update a document.</typeparam>
        /// <typeparam name="TResponse">Type of the returned document, only applies when
        /// <see cref="PatchDocumentOptions.ReturnNew"/> or <see cref="PatchDocumentOptions.ReturnOld"/>
        /// are used.</typeparam>
        /// <param name="documentId"></param>
        /// <param name="body"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PatchDocumentResponse<TResponse>> PatchDocumentAsync<TPatch, TResponse>(
            string documentId,
            TPatch body,
            PatchDocumentOptions query = null,
            CancellationToken cancellationToken = default)
        {
            ValidateDocumentId(documentId);
            return await PatchRequestAsync(ApiRootPath + "/" + documentId,
                response => new PatchDocumentResponse<TResponse>(response), body, query, cancellationToken);
        }

        /// <summary>
        /// Like GET, but only returns the header fields and not the body. You
        /// can use this call to get the current revision of a document or check if
        /// the document was deleted.
        /// HEAD /_api/document/{document-handle}
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="documentKey"></param>
        /// <param name="headers"></param>
        /// <remarks>
        /// 200: is returned if the document was found. 
        /// 304: is returned if the “If-None-Match” header is given and the document has the same version. 
        /// 404: is returned if the document or collection was not found. 
        /// 412: is returned if an “If-Match” header is given and the found document has a different version. The response will also contain the found document’s current revision in the Etag header.
        /// </remarks>
        /// <returns></returns>
        public async Task<HeadDocumentResponse> HeadDocumentAsync(
            string collectionName,
            string documentKey,
            HeadDocumentHeader headers = null,
            CancellationToken cancellationToken = default)
        {
            return await HeadDocumentAsync(
                $"{WebUtility.UrlEncode(collectionName)}/{WebUtility.UrlEncode(documentKey)}", headers,
                cancellationToken);
        }

        /// <summary>
        /// Like GET, but only returns the header fields and not the body. You
        /// can use this call to get the current revision of a document or check if
        /// the document was deleted.
        /// HEAD/_api/document/{document-handle}
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="headers">Object containing a dictionary of Header keys and values</param>
        /// <exception cref="ArgumentException">Document ID is invalid.</exception>
        /// <remarks>
        /// 200: is returned if the document was found. 
        /// 304: is returned if the “If-None-Match” header is given and the document has the same version. 
        /// 404: is returned if the document or collection was not found. 
        /// 412: is returned if an “If-Match” header is given and the found document has a different version. The response will also contain the found document’s current revision in the Etag header.
        /// </remarks>
        /// <returns></returns>
        public async Task<HeadDocumentResponse> HeadDocumentAsync(string documentId, HeadDocumentHeader headers = null, CancellationToken cancellationToken = default)
        {
            ValidateDocumentId(documentId);
            string uri = ApiRootPath + "/" + documentId;
            WebHeaderCollection headerCollection = headers == null ? new WebHeaderCollection() : headers.ToWebHeaderCollection();
            using (var response = await Transport.HeadAsync(uri, headerCollection, cancellationToken))
            {
                if (response.IsSuccessStatusCode)
                {
                    return new HeadDocumentResponse(response.StatusCode, response.Headers.ETag);
                }
                return new HeadDocumentResponse(response.Headers.ETag, new ApiResponse(true, response.StatusCode, null, null));
            }
        }
    }
}
