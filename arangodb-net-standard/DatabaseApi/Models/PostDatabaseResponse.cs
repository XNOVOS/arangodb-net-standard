using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.DatabaseApi.Models
{
    /// <summary>
    /// Represents the content of the response returned
    /// by an endpoint that creates a new database.
    /// </summary>
    public class PostDatabaseResponse : ResponseBase
    {
        [JsonConstructor]
        public PostDatabaseResponse(bool error, HttpStatusCode code, bool result) : base(
            new ApiResponse(error, code, null, null))
        {
            Result = result;
        }

        /// <summary>
        /// Indicates that the database was created. Always true.
        /// </summary>
        public bool Result { get; }

        public PostDatabaseResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
