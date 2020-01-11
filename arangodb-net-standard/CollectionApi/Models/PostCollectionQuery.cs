using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace ArangoDBNetStandard.CollectionApi.Models
{
    public class PostCollectionQuery : RequestOptionsBase
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

        protected override void PrepareQueryStringValues(IDictionary<string, string> values)
        {
            if (WaitForSyncReplication.HasValue)
                values.Add(nameof(WaitForSyncReplication).ToCamelCase(), (WaitForSyncReplication.Value ? 1 : 0).ToString());
            if (EnforceReplicationFactor.HasValue)
                values.Add(nameof(EnforceReplicationFactor).ToCamelCase(), (EnforceReplicationFactor.Value ? 1 : 0).ToString());
        }
    }
}