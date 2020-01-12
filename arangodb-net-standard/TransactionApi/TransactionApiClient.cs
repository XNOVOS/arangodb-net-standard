using System.Threading;
using ArangoDBNetStandard.Serialization;
using ArangoDBNetStandard.TransactionApi.Models;
using ArangoDBNetStandard.Transport;
using System.Threading.Tasks;

namespace ArangoDBNetStandard.TransactionApi
{
    /// <summary>
    /// Provides access to ArangoDB transaction API.
    /// </summary>
    public class TransactionApiClient: ApiClientBase, ITransactionApiClient
    {
        protected override string ApiRootPath => "_api/transaction";

        /// <summary>
        /// Create an instance of <see cref="TransactionApiClient"/>
        /// using the provided transport layer and the default JSON serialization.
        /// </summary>
        /// <param name="client"></param>
        public TransactionApiClient(IApiClientTransport transport)
            : base(transport, new JsonNetApiClientSerialization())
        {
        }

        /// <summary>
        /// Create an instance of <see cref="TransactionApiClient"/>
        /// using the provided transport and serialization layers.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="serializer"></param>
        public TransactionApiClient(IApiClientTransport transport, IApiClientSerialization serializer)
            : base(transport, serializer)
        {
        }

        /// <summary>
        /// POST a transaction to ArangoDB.
        /// </summary>
        /// <typeparam name="T">Type to use for deserializing the object returned by the transaction function.</typeparam>
        /// <param name="body">Object containing information to submit in the POST transaction request.</param>
        /// <returns>Response from ArangoDB after processing the request.</returns>
        public virtual async Task<PostTransactionResponse<T>> PostTransactionAsync<T>(PostTransactionBody body, CancellationToken cancellationToken = default)
        {
            return await PostRequestAsync(ApiRootPath, response => new PostTransactionResponse<T>(response), body, null,
                cancellationToken);
        }
    }
}
