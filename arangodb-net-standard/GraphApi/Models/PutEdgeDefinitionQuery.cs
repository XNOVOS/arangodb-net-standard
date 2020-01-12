using System.Collections.Generic;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class PutEdgeDefinitionQuery : RequestOptionsBase
    {
        public bool? WaitForSync { get; set; }
    }
}
