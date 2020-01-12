using ArangoDBNetStandard.CursorApi.Models;
using ArangoDBNetStandard.Serialization;
using ArangoDBNetStandard.Transport;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ArangoDBNetStandard.CursorApi
{
    /// <summary>
    /// ArangoDB Cursor API.
    /// </summary>
    public class CursorApiClient : ApiClientBase, ICursorApiClient
    {
        protected override string ApiRootPath => "_api/cursor";

        /// <summary>
        /// Creates an instance of <see cref="CursorApiClient"/>
        /// using the provided transport layer and the default JSON serialization.
        /// </summary>
        /// <param name="client"></param>
        public CursorApiClient(IApiClientTransport transport)
            : base(transport, new JsonNetApiClientSerialization())
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="CursorApiClient"/>
        /// using the provided transport and serialization layers.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="serializer"></param>
        public CursorApiClient(IApiClientTransport transport, IApiClientSerialization serializer)
            : base(transport, serializer)
        {
        }

        /// <summary>
        /// Execute an AQL query, creating a cursor which can be used to page query results.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="bindVars"></param>
        /// <param name="options"></param>
        /// <param name="count"></param>
        /// <param name="batchSize"></param>
        /// <param name="cache"></param>
        /// <param name="memoryLimit"></param>
        /// <param name="ttl"></param>
        /// <returns></returns>
        public virtual async Task<CursorResponse<T>> PostCursorAsync<T>(
                string query,
                Dictionary<string, object> bindVars = null,
                PostCursorOptions options = null,
                bool? count = null,
                long? batchSize = null,
                bool? cache = null,
                long? memoryLimit = null,
                int? ttl = null,
                CancellationToken cancellationToken = default)
        {
            return await PostCursorAsync<T>(new PostCursorBody
            {
                Query = query,
                BindVars = bindVars,
                Options = options,
                Count = count,
                BatchSize = batchSize,
                Cache = cache,
                MemoryLimit = memoryLimit,
                Ttl = ttl,
            }, cancellationToken);
        }

        /// <summary>
        /// Execute an AQL query, creating a cursor which can be used to page query results.
        /// </summary>
        /// <param name="postCursorBody">Object encapsulating options and parameters of the query.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<CursorResponse<T>> PostCursorAsync<T>(PostCursorBody postCursorBody,
            CancellationToken cancellationToken = default)
        {
            return await PostRequestAsync(ApiRootPath, response => new CursorResponse<T>(response), postCursorBody,
                null, cancellationToken);
        }

        /// <summary>
        /// Deletes an existing cursor and frees the resources associated with it.
        /// DELETE /_api/cursor/{cursor-identifier}
        /// </summary>
        /// <param name="cursorId">The id of the cursor to delete.</param>
        /// <returns></returns>
        public virtual async Task<DeleteCursorResponse> DeleteCursorAsync(string cursorId, CancellationToken cancellationToken = default)
        {
            return await DeleteRequestAsync($"{ApiRootPath}/{WebUtility.UrlEncode(cursorId)}",
                response => new DeleteCursorResponse(response), null, cancellationToken);
        }

        /// <summary>
        /// Advances an existing query cursor and gets the next set of results.
        /// </summary>
        /// <typeparam name="T">Result type to deserialize to</typeparam>
        /// <param name="cursorId">ID of the existing query cursor.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<PutCursorResponse<T>> PutCursorAsync<T>(string cursorId, CancellationToken cancellationToken = default)
        {
            return await PutRequestAsync(ApiRootPath + "/" + WebUtility.UrlEncode(cursorId),
                response => new PutCursorResponse<T>(response), null, null, cancellationToken);
        }
    }
}
