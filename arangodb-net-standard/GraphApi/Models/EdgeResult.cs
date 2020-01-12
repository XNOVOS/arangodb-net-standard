using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    /// <summary>
    /// Represents the internal attributes of an edge returned in a response.
    /// </summary>
    public class EdgeResult
    {
        [JsonConstructor]
        public EdgeResult(string _id, string _key, string _rev)
        {
            this._id = _id;
            this._key = _key;
            this._rev = _rev;
        }

        public string _id { get; }

        public string _key { get; }

        public string _rev { get; }
    }
}
