using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    public class DeleteDocumentsQuery : RequestOptionsBase
    {
        public bool? WaitForSync { get; set; }

        public bool? ReturnOld { get; set; }

        public bool? Silent { get; set; }

        protected override void PrepareQueryStringValues(IDictionary<string, string> values)
        {
            if (WaitForSync.HasValue)
                values.Add(nameof(WaitForSync).ToCamelCase(), WaitForSync.ToString().ToLowerInvariant());
            if (ReturnOld.HasValue)
                values.Add(nameof(ReturnOld).ToCamelCase(), ReturnOld.ToString().ToLowerInvariant());
            if (Silent.HasValue)
                values.Add(nameof(Silent).ToCamelCase(), Silent.ToString().ToLowerInvariant());
        }
    }
}