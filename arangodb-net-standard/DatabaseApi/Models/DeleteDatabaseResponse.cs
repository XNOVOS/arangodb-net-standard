using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.DatabaseApi.Models
{
    public class DeleteDatabaseResponse : ResponseBase
    {
        [JsonConstructor]
        public DeleteDatabaseResponse(bool error, HttpStatusCode code, bool result) : base(
            new ApiResponse(error, code, null, null))
        {
            Result = result;
        }

        /// <summary>
        /// Indicates that the database was created. Always true.
        /// </summary>
        public bool Result { get; }

        public DeleteDatabaseResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
