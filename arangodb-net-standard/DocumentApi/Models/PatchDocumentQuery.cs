using System.Collections.Generic;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    public class PatchDocumentOptions : PatchDocumentsOptions
    {
        public bool? Silent { get; set; }
    }
}
