using System.Threading;
using System.Threading.Tasks;
using ArangoDBNetStandard.TransactionApi.Models;

namespace ArangoDBNetStandard.TransactionApi
{
    public interface ITransactionApiClient
    {
        /// <summary>
        /// POST a transaction to ArangoDB.
        /// </summary>
        /// <typeparam name="T">Type to use for deserializing the object returned by the transaction function.</typeparam>
        /// <param name="body">Object containing information to submit in the POST transaction request.</param>
        /// <returns>Response from ArangoDB after processing the request.</returns>
        Task<PostTransactionResponse<T>> PostTransactionAsync<T>(PostTransactionBody body, CancellationToken cancellationToken = default);
    }
}