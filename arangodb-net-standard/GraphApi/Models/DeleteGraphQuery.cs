using System.Collections.Generic;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class DeleteGraphOptions : RequestOptionsBase
    {
        public bool? DropCollections { get; set; }
    }
}
