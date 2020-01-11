using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ArangoDBNetStandard.CollectionApi.Models
{
    public class GetCollectionsQuery : RequestOptionsBase
    {
        public bool? ExcludeSystem { get; set; }

        protected override void PrepareQueryStringValues(IDictionary<string, string> values)
        {
            if (ExcludeSystem.HasValue)
            {
                values.Add(nameof(ExcludeSystem).ToCamelCase(), ExcludeSystem.ToString().ToLowerInvariant());
            }
        }
    }
}
