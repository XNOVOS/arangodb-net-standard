using System.Collections.Generic;
using System.Collections.ObjectModel;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    /// <summary>
    /// Options used when calling ArangoDB PUT document endpoint.
    /// </summary>
    public class PutDocumentsOptions : RequestOptionsBase
    {
        /// <summary>
        /// Whether to wait until the new documents have been synced to disk.
        /// </summary>
        public bool? WaitForSync { get; set; }

        /// <summary>
        /// By default, or if this is set to true, the _rev attributes in
        /// the given documents are ignored. If this is set to false, then
        /// any _rev attribute given in a body document is taken as a
        /// precondition. The document is only replaced if the current revision
        /// is the one specified.
        /// </summary>
        public bool? IgnoreRevs { get; set; }

        /// <summary>
        /// Whether to return the complete previous revision of the changed
        /// documents under <see cref="PostDocumentResponse{T}.Old"/>.
        /// </summary>
        public bool? ReturnOld { get; set; }

        /// <summary>
        /// Whether to return the complete new revision of the changed
        /// documents under <see cref="PostDocumentResponse{T}.New"/>.
        /// </summary>
        public bool? ReturnNew { get; set; }
    }
}