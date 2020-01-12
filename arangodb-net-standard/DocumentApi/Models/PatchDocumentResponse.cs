using ArangoDBNetStandard.Models;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.DocumentApi.Models
{
    public class PatchDocumentResponse<T> : ResponseBase
    {
        public T New { get; }

        public T Old { get; }

        public string _key { get; }

        public string _rev { get; }

        public string _oldRev { get; }

        public string _id { get; }

        [JsonConstructor]
        public PatchDocumentResponse(string _key, string _rev, string _id, string _oldRev, T @new, T old) : base(ApiResponse.SuccessfulResponse)
        {
            this._key = _key;
            this._rev = _rev;
            this._id = _id;
            this._oldRev = _oldRev;
            New = @new;
            Old = old;
        }
        public PatchDocumentResponse(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}
