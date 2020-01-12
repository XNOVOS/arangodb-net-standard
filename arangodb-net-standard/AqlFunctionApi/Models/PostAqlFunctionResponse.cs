using System.Net;
using ArangoDBNetStandard.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.AqlFunctionApi.Models
{
    /// <summary>
    /// Represents a response returned when creating an AQL user function.
    /// </summary>
    public class PostAqlFunctionResponse : ResponseBase
    {
        /// <summary>
        /// Indicates whether the function was newly created.
        /// </summary>
        public bool IsNewlyCreated { get; }

        [JsonConstructor]
        public PostAqlFunctionResponse(bool error, HttpStatusCode code, bool isNewlyCreated) : base(new ApiResponse(error, code))
        {
            IsNewlyCreated = isNewlyCreated;
        }

        public PostAqlFunctionResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }
    }
}
