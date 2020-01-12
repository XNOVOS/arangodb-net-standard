using System.Collections.Generic;
using System.Collections.ObjectModel;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.CollectionApi.Models
{
    public class GetCollectionsOptions : RequestOptionsBase
    {
        public bool? ExcludeSystem { get; set; }
    }
}
