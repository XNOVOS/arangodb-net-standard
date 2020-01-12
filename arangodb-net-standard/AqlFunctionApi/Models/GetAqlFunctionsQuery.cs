using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.AqlFunctionApi.Models
{
    /// <summary>
    /// Represents query parameters used when getting all AQL user functions.
    /// </summary>
    public class GetAqlFunctionsQuery : RequestOptionsBase
    {
        /// <summary>
        /// Returns all registered AQL user functions from this namespace under result.
        /// </summary>
        public string Namespace { get; set; }
    }
}
