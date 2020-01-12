using ArangoDBNetStandard.AqlFunctionApi.Models;
using ArangoDBNetStandard.Serialization;
using ArangoDBNetStandard.Transport;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ArangoDBNetStandard.AqlFunctionApi
{
    /// <summary>
    /// A client to interact with ArangoDB HTTP API endpoints
    /// for AQL user functions management.
    /// </summary>
    public class AqlFunctionApiClient : ApiClientBase, IAqlFunctionApiClient
    {
        protected override string ApiRootPath => "_api/aqlfunction";

        /// <summary>
        /// Create an instance of <see cref="AqlFunctionApiClient"/>
        /// using the provided transport layer and the default JSON serialization.
        /// </summary>
        /// <param name="transport"></param>
        public AqlFunctionApiClient(IApiClientTransport transport)
            : base(transport, new JsonNetApiClientSerialization())
        {
        }

        /// <summary>
        /// Create an instance of <see cref="AqlFunctionApiClient"/>
        /// using the provided transport and serialization layers.
        /// </summary>
        /// <param name="transport"></param>
        /// <param name="serializer"></param>
        public AqlFunctionApiClient(IApiClientTransport transport, IApiClientSerialization serializer)
            : base(transport, serializer)
        {
        }

        /// <summary>
        /// Create a new AQL user function.
        /// POST /_api/aqlfunction
        /// </summary>
        /// <param name="body">The body of the request containing required properties.</param>
        /// <returns></returns>
        public virtual async Task<PostAqlFunctionResponse> PostAqlFunctionAsync(PostAqlFunctionBody body, CancellationToken cancellationToken = default)
        {
            return await PostRequestAsync(ApiRootPath, response => new PostAqlFunctionResponse(response), body, null,
                cancellationToken);
        }

        /// <summary>
        /// Removes an existing AQL user function or function group, identified by name.
        /// DELETE /_api/aqlfunction/{name}
        /// </summary>
        /// <param name="name">The name of the function or function group (namespace).</param>
        /// <param name="query">The query parameters of the request.</param>
        /// <returns></returns>
        public virtual async Task<DeleteAqlFunctionResponse> DeleteAqlFunctionAsync(
            string name,
            DeleteAqlFunctionQuery query = null,
            CancellationToken cancellationToken = default)
        {
            return await DeleteRequestAsync($"{ApiRootPath}{'/'}{WebUtility.UrlEncode(name)}",
                response => new DeleteAqlFunctionResponse(response), query, cancellationToken);
        }

        /// <summary>
        /// Get all registered AQL user functions.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<GetAqlFunctionsResponse> GetAqlFunctionsAsync(GetAqlFunctionsQuery query = null, CancellationToken cancellationToken = default)
        {
            return await GetRequestAsync(ApiRootPath, response => new GetAqlFunctionsResponse(response), query,
                cancellationToken);
        }
    }
}
