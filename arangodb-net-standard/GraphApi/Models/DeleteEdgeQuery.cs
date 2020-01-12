using System.Collections.Generic;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class DeleteEdgeQuery : RequestOptionsBase
    {
        public bool? WaitForSync { get; set; }

        public bool? ReturnOld { get; set; }
    }
}
