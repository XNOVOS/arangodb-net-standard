using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    /// <summary>
    /// Options used when calling ArangoDB POST document endpoint.
    /// </summary>
    public class PostDocumentsQuery : RequestOptionsBase
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

        protected override void PrepareQueryStringValues(IDictionary<string, string> values)
        {
            if (WaitForSync.HasValue)
                values.Add(nameof(WaitForSync).ToCamelCase(), WaitForSync.ToString().ToLowerInvariant());
            if (ReturnNew.HasValue)
                values.Add(nameof(ReturnNew).ToCamelCase(), ReturnNew.ToString().ToLowerInvariant());
            if (ReturnOld.HasValue)
                values.Add(nameof(ReturnOld).ToCamelCase(), ReturnOld.ToString().ToLowerInvariant());
            if (Silent.HasValue)
                values.Add(nameof(Silent).ToCamelCase(), Silent.ToString().ToLowerInvariant());
            if (Overwrite.HasValue)
                values.Add(nameof(Overwrite).ToCamelCase(), Overwrite.ToString().ToLowerInvariant());
        }
    }
}