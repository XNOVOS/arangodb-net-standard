using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    public class PatchDocumentsQuery : RequestOptionsBase
    {
        public bool? KeepNull { get; set; }

        public bool? MergeObjects { get; set; }

        public bool? WaitForSync { get; set; }

        public bool? IgnoreRevs { get; set; }

        public bool? ReturnOld { get; set; }

        public bool? ReturnNew { get; set; }

        protected override void PrepareQueryStringValues(IDictionary<string, string> values)
        {
            if (WaitForSync.HasValue)
                values.Add(nameof(WaitForSync).ToCamelCase(), WaitForSync.ToString().ToLowerInvariant());
            if (ReturnOld.HasValue)
                values.Add(nameof(ReturnOld).ToCamelCase(), ReturnOld.ToString().ToLowerInvariant());
            if (KeepNull.HasValue)
                values.Add(nameof(KeepNull).ToCamelCase(), KeepNull.ToString().ToLowerInvariant());
            if (MergeObjects.HasValue)
                values.Add(nameof(MergeObjects).ToCamelCase(), MergeObjects.ToString().ToLowerInvariant());
            if (ReturnNew.HasValue)
                values.Add(nameof(ReturnNew).ToCamelCase(), ReturnNew.ToString().ToLowerInvariant());
            if (IgnoreRevs.HasValue)
                values.Add(nameof(IgnoreRevs).ToCamelCase(), IgnoreRevs.ToString().ToLowerInvariant());
        }
    }
}
