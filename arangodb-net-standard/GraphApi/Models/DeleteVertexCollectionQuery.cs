using System.Collections.Generic;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class DeleteVertexCollectionOptions : RequestOptionsBase
    {
        public bool? DropCollection { get; set; }
    }
}
