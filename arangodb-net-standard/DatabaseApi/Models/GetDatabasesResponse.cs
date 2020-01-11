using System.Collections.Generic;
using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.DatabaseApi.Models
{
    /// <summary>
    /// Represents the content of the response returned
    /// by an endpoint that gets the list of databases.
    /// </summary>
    public class GetDatabasesResponse : ResponseBase
    {
        /// <summary>
        /// The list of databases.
        /// </summary>
        public IReadOnlyList<string> Results { get; }

        public GetDatabasesResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }

        [JsonConstructor]
        public GetDatabasesResponse(bool error, HttpStatusCode code, IEnumerable<string> result) : base(new ApiResponse(error, code, null, null))
        {
            Results = new List<string>(result).AsReadOnly();
        }
    }
}
