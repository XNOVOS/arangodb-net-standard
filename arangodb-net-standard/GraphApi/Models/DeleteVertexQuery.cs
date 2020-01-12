using System.Collections.Generic;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class DeleteVertexQuery : RequestOptionsBase
    {
        public bool? WaitForSync { get; set; }

        public bool? ReturnOld { get; set; }
    }
}
