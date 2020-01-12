using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard.CollectionApi.Models
{
    public class PostCollectionOptions : RequestOptionsBase
    {
        /// <summary>
        /// Default is true which means the server will only report success back to the
        /// client if all replicas have created the collection. Set to false if you want
        /// faster server responses and don’t care about full replication.
        /// </summary>
        public bool? WaitForSyncReplication { get; set; }

        /// <summary>
        /// Default is true which means the server will check if there are enough replicas
        /// available at creation time and bail out otherwise. Set to false to disable
        /// this extra check.
        /// </summary>
        public bool? EnforceReplicationFactor { get; set; }
    }
}