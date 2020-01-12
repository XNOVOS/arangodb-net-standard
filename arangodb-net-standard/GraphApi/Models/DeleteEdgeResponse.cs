using System.Net;
using ArangoDBNetStandard.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ArangoDBNetStandard.GraphApi.Models
{
    /// <summary>
    /// Represents a response containing information about a deleted edge in a graph.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DeleteEdgeResponse<T> : ResponseBase
    {
        /// <summary>
        /// The complete deleted edge document.
        /// Includes all attributes stored before the delete operation.
        /// Will only be present if <see cref="DeleteEdgeQuery.ReturnOld"/> is true.
        /// </summary>
        public T Old { get; }

        /// <summary>
        /// Is set to true if the edge was successful removed.
        /// </summary>
        public bool Removed { get; }
        
        public DeleteEdgeResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
        }

        [JsonConstructor]
        public DeleteEdgeResponse(T old, bool removed, HttpStatusCode code, bool error) : base(new ApiResponse(error, code))
        {
            Old = old;
            Removed = removed;
        }
    }
}
