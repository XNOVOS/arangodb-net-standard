using ArangoDBNetStandard.DocumentApi.Models;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.AuthApi.Models
{
    public class JwtTokenResponse : ResponseBase
    {
        /// <summary>
        /// JWT Token
        /// </summary>
        public string Jwt { get; }

        [JsonConstructor]
        public JwtTokenResponse(string jwt) : base(null)
        {
            Jwt = jwt;
        }
        public JwtTokenResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
