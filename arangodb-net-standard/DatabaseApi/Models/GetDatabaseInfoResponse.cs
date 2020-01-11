using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.DatabaseApi.Models
{
    /// <summary>
    /// Represents a response containing information about the current database.
    /// </summary>
    public class GetCurrentDatabaseInfoResponse : ResponseBase
    {
        /// <summary>
        /// The database information.
        /// </summary>
        public CurrentDatabaseInfo Result { get; }

        public GetCurrentDatabaseInfoResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }

        [JsonConstructor]
        public GetCurrentDatabaseInfoResponse(bool error, HttpStatusCode code, CurrentDatabaseInfo result) : base(new ApiResponse(error, code, null, null))
        {
            Result = result;
        }
    }
}
