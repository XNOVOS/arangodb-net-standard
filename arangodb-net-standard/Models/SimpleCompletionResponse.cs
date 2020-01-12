using System.Net;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.Models
{
    public class SimpleCompletionResponse : ResponseBase
    {
        [JsonConstructor]
        public SimpleCompletionResponse(bool error, HttpStatusCode code) : base(new ApiResponse(error, code))
        {
        }

        public SimpleCompletionResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }
    }
}