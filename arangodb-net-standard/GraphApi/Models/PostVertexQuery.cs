using System.Collections.Generic;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class PostVertexOptions : RequestOptionsBase
    {
        public bool? WaitForSync { get; set; }

        public bool? ReturnNew { get; set; }
    }
}
