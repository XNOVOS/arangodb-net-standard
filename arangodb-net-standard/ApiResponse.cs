using System.Net;

namespace ArangoDBNetStandard
{
    /// <summary>
    /// ArangoDB API error model
    /// </summary>
    public class ApiResponse
    {
        public ApiResponse(bool? error, HttpStatusCode? code, string errorMessage = null, int? errorNum = null)
        {
            if (error.HasValue)
                Error = error.Value;
            ErrorMessage = errorMessage;
            if (errorNum.HasValue)
                ErrorNum = errorNum.Value;
            if (code.HasValue)
                Code = code.Value;
        }

        /// <summary>
        /// Whether this is an error response (always true).
        /// </summary>
        public bool Error { get; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// ArangoDB error number.
        /// See https://www.arangodb.com/docs/stable/appendix-error-codes.html for error numbers and descriptions.
        /// </summary>
        public int ErrorNum { get; }

        /// <summary>
        /// HTTP status code.
        /// </summary>
        public HttpStatusCode Code { get; }

        public static ApiResponse SuccessfulResponse => new ApiResponse(false, HttpStatusCode.OK);
    }
}
