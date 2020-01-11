using System.Collections.Generic;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class DeleteEdgeDefinitionQuery : RequestOptionsBase
    {
        public bool? WaitForSync { get; set; }

        public bool? DropCollections { get; set; }

        protected override void PrepareQueryStringValues(IDictionary<string, string> values)
        {
            if (WaitForSync.HasValue)
                values.Add(nameof(WaitForSync).ToCamelCase(), WaitForSync.ToString().ToLowerInvariant());
            if (DropCollections.HasValue)
                values.Add(nameof(DropCollections).ToCamelCase(), DropCollections.ToString().ToLowerInvariant());
        }
    }
}
