using System.Collections.Generic;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    public class PatchDocumentQuery : PatchDocumentsQuery
    {
        public bool? Silent { get; set; }

        protected override void PrepareQueryStringValues(IDictionary<string, string> values)
        {
            base.PrepareQueryStringValues(values);
            if (Silent.HasValue)
                values.Add(nameof(Silent).ToCamelCase(), Silent.ToString().ToLowerInvariant());
        }
    }
}
