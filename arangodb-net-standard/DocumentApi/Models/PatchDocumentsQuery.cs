using System.Collections.Generic;
using System.Collections.ObjectModel;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    public class PatchDocumentsOptions : RequestOptionsBase
    {
        public bool? KeepNull { get; set; }

        public bool? MergeObjects { get; set; }

        public bool? WaitForSync { get; set; }

        public bool? IgnoreRevs { get; set; }

        public bool? ReturnOld { get; set; }

        public bool? ReturnNew { get; set; }
    }
}
