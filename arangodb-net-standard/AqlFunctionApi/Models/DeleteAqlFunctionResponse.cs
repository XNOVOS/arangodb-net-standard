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
    /// Represents a response containing the number of deleted AQL user functions.
    /// </summary>
    public class DeleteAqlFunctionResponse : ResponseBase
    {
        /// <summary>
        /// The number of deleted user functions,
        /// always 1 when <see cref="DeleteAqlFunctionQuery.Group"/> is set to false.
        /// Any number >= 0 when <see cref="DeleteAqlFunctionQuery.Group"/> is set to true.
        /// </summary>
        public int DeletedCount { get; }

        [JsonConstructor]
        public DeleteAqlFunctionResponse( bool error, HttpStatusCode code, int deletedCount) : base(new ApiResponse(error, code))
        {
            DeletedCount = deletedCount;
        }

        public DeleteAqlFunctionResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }
    }
}
