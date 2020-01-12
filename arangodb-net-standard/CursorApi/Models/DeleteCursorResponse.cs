using System.Net;
using ArangoDBNetStandard.DocumentApi.Models;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.CursorApi.Models
{
    /// <summary>
    /// Represents a response returned after deleting a cursor.
    /// </summary>
    public class DeleteCursorResponse : ResponseBase
    {
        [JsonConstructor]
        public DeleteCursorResponse(bool error, HttpStatusCode code, string id) : base(new ApiResponse(error, code, null, null))
        {
            Id = id;
        }

        public DeleteCursorResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }

        /// <summary>
        /// The id of the cursor.
        /// </summary>
        public string Id { get; }
    }
}
