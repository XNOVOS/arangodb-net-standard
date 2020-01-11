using Newtonsoft.Json;

namespace ArangoDBNetStandard.CollectionApi.Models
{
    public class CollectionKeyOptions
    {
        [JsonConstructor]
        public CollectionKeyOptions(bool allowUserKeys, long increment, long offset, string type)
        {
            AllowUserKeys = allowUserKeys;
            Increment = increment;
            Offset = offset;
            Type = type;
        }

        public bool AllowUserKeys { get; }

        public long Increment { get; }

        public long Offset { get; }

        public string Type { get; }
    }
}
