using System.Net;
using ArangoDBNetStandard.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.UserApi.Models
{
    public class DeleteUserResponse : SimpleCompletionResponse
    {
        [JsonConstructor]
        public DeleteUserResponse(bool error, HttpStatusCode code) : base(error, code)
        {
        }

        public DeleteUserResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }
    }
}
