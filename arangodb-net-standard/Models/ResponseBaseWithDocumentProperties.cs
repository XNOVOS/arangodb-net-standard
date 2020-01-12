namespace ArangoDBNetStandard.Models
{
    public abstract class ResponseBaseWithDocumentProperties : ResponseBase
    {
        public string _key { get; }

        /// <summary>
        /// ArangoDB document ID.
        /// </summary>
        public string _id { get; }

        /// <summary>
        /// ArangoDB document revision tag.
        /// </summary>
        public string _rev { get; }

        protected ResponseBaseWithDocumentProperties(string key, string id, string rev) : base(null)
        {
            _key = key;
            _id = id;
            _rev = rev;
        }

        protected ResponseBaseWithDocumentProperties(ApiResponse errorDetails) : base(errorDetails)
        {
        }
    }
}