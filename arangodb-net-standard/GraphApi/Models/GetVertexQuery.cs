using System.Collections.Generic;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class GetVertexQuery : RequestOptionsBase
    {
        public bool? Rev { get; set; }
    }
}
