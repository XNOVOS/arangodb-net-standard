using System;
using System.Collections.Generic;
using System.Text;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class PatchEdgeQuery : RequestOptionsBase
    {
        public bool? WaitForSync { get; set; }

        public bool? KeepNull { get; set; }

        public bool? ReturnOld { get; set; }

        public bool? ReturnNew { get; set; }
    }
}
