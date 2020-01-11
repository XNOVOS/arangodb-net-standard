using System.Collections.Generic;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    /// <summary>
    /// Represents graph information returned in the response
    /// of graph management endpoints.
    /// </summary>
    public class GraphResult
    {
        [JsonConstructor]
        public GraphResult(string id, string key, string rev, string name, string smartGraphAttribute, int replicationFactor, IEnumerable<string> orphanCollections, int numberOfShards, bool isSmart, IEnumerable<EdgeDefinition> edgeDefinitions)
        {
            _id = id;
            _key = key;
            _rev = rev;
            Name = name;
            SmartGraphAttribute = smartGraphAttribute;
            ReplicationFactor = replicationFactor;
            OrphanCollections = orphanCollections;
            NumberOfShards = numberOfShards;
            IsSmart = isSmart;
            EdgeDefinitions = edgeDefinitions;
        }

        /// <summary>
        /// The internal id value of this graph.
        /// </summary>
        public string _id { get; }

        /// <summary>
        /// The key of this graph (corresponding to the name).
        /// </summary>
        public string _key { get; }

        /// <summary>
        /// The revision of this graph.
        /// Can be used to make sure to not override concurrent modifications to this graph.
        /// </summary>
        public string _rev { get; }

        /// <summary>
        /// The name of the graph.
        /// Note: This property is null for <see cref="GraphApiClient.GetGraphsAsync"/> in ArangoDB 4.5.2 and below,
        /// in which case you can use <see cref="_key"/> instead.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The name of the sharding attribute in smart graph case (Enterprise Edition only).
        /// </summary>
        public string SmartGraphAttribute { get; }

        /// <summary>
        /// The replication factor used for every new collection in the graph.
        /// </summary>
        public int ReplicationFactor { get; }

        /// <summary>
        /// A list of additional vertex collections.
        /// Documents within these collections do not have edges within this graph.
        /// </summary>
        public IEnumerable<string> OrphanCollections { get; }

        /// <summary>
        /// Number of shards created for every new collection in the graph.
        /// </summary>
        public int NumberOfShards { get; }

        /// <summary>
        /// Indicates whether the graph is a SmartGraph (Enterprise Edition only).
        /// </summary>
        public bool IsSmart { get; }

        /// <summary>
        /// A list of definitions for the relations of the graph.
        /// </summary>
        public IEnumerable<EdgeDefinition> EdgeDefinitions { get; }
    }
}
