using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    public class PostVertexResult
    {
        [JsonConstructor]
        public PostVertexResult(string _key, string _id, string _rev)
        {
            this._key = _key;
            this._id = _id;
            this._rev = _rev;
        }

        public string _key { get; }

        public string _id { get; }

        public string _rev { get; }
    }
}
