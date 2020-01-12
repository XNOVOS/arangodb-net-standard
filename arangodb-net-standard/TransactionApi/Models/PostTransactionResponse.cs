using System.Net;
using ArangoDBNetStandard.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.TransactionApi.Models
{
    /// <summary>
    /// Response from ArangoDB after executing a transaction.
    /// </summary>
    /// <typeparam name="T">Type used to deserialize the returned object from the transaction function.</typeparam>
    public class PostTransactionResponse<T> : ResponseBase
    {
        /// <summary>
        /// Deserialized result from the transaction function.
        /// </summary>
        public T Result { get; }

        [JsonConstructor]
        public PostTransactionResponse(bool error, HttpStatusCode code, T result) : base(new ApiResponse(error, code))
        {
            Result = result;
        }

        public PostTransactionResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }
    }
}