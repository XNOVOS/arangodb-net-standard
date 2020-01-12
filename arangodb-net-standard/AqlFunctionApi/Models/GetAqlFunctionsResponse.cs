using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ArangoDBNetStandard.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.AqlFunctionApi.Models
{
    /// <summary>
    /// Represents a response returned when getting all AQL user functions.
    /// </summary>
    [JsonObject]
    public class GetAqlFunctionsResponse : ListResponse<AqlFunctionResult>
    {
        [JsonConstructor]
        public GetAqlFunctionsResponse(IEnumerable<AqlFunctionResult> result, bool error, HttpStatusCode code) : base(result, new ApiResponse(error, code))
        {
        }

        public GetAqlFunctionsResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }
    }
}
