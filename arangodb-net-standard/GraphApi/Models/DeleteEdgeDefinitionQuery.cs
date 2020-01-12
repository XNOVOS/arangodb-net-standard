using System.Collections.Generic;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class DeleteEdgeDefinitionOptions : RequestOptionsBase
    {
        public bool? WaitForSync { get; set; }

        public bool? DropCollections { get; set; }
    }
}
