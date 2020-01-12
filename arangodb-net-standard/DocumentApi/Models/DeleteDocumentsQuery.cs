using System.Collections.Generic;
using System.Collections.ObjectModel;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    public class DeleteDocumentsOptions : RequestOptionsBase
    {
        public bool? WaitForSync { get; set; }

        public bool? ReturnOld { get; set; }

        public bool? Silent { get; set; }
    }
}