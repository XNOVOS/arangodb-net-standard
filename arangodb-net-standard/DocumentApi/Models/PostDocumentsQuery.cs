using System.Collections.Generic;
using System.Collections.ObjectModel;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    /// <summary>
    /// Options used when calling ArangoDB POST document endpoint.
    /// </summary>
    public class PostDocumentsOptions : RequestOptionsBase
    {
        public bool? WaitForSync { get; set; }

        /// <summary>
        /// Whether to return a full copy of the new document.
        /// </summary>
        public bool? ReturnNew { get; set; }

        /// <summary>
        /// Whether to return a full copy of the old document.
        /// </summary>
        public bool? ReturnOld { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public bool? Silent { get; set; }

        /// <summary>
        /// If a document already exists, whether to overwrite (replace) the document rather than respond with error.
        /// </summary>
        public bool? Overwrite { get; set; }
    }
}