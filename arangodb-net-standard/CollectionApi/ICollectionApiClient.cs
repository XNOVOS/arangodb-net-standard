using System.Threading;
using ArangoDBNetStandard.CollectionApi.Models;
using System.Threading.Tasks;

namespace ArangoDBNetStandard.CollectionApi
{
    /// <summary>
    /// Defines a client to access the ArangoDB Collections API.
    /// </summary>
    public interface ICollectionApiClient
    {
        Task<PostCollectionResponse> PostCollectionAsync(
           PostCollectionBody body,
           PostCollectionQuery options = null,
           CancellationToken cancellationToken = default);

        Task<DeleteCollectionResponse> DeleteCollectionAsync(string collectionName,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Truncates a collection, i.e. removes all documents in the collection.
        /// PUT/_api/collection/{collection-name}/truncate
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        Task<TruncateCollectionResponse> TruncateCollectionAsync(string collectionName,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets count of documents in a collection.
        /// GET/_api/collection/{collection-name}/count
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        Task<GetCollectionCountResponse> GetCollectionCountAsync(string collectionName,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all collections.
        /// GET/_api/collection
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<GetCollectionsResponse> GetCollectionsAsync(GetCollectionsQuery query = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the requested collection.
        /// GET/_api/collection/{collection-name}
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        Task<GetCollectionResponse> GetCollectionAsync(string collectionName,
            CancellationToken cancellationToken = default);

        /// Read properties of a collection.
        /// GET /_api/collection/{collection-name}/properties
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<GetCollectionPropertiesResponse> GetCollectionPropertiesAsync(string collectionName,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Rename a collection.
        /// PUT /_api/collection/{collection-name}/rename
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="body"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<RenameCollectionResponse> RenameCollectionAsync(string collectionName,
            RenameCollectionBody body, CancellationToken cancellationToken);

        /// <summary>
        /// Get a revision of the collection. 
        /// GET /_api/collection/{collection-name}/revision
        /// </summary>
        /// <param name="collectionName">Name of the collection</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<GetCollectionRevisionResponse> GetCollectionRevisionAsync(string collectionName,
            CancellationToken cancellationToken);

        /// <summary>
        /// Changes the properties of a collection
        /// PUT /_api/collection/{collection-name}/properties
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="body"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PutCollectionPropertyResponse> PutCollectionPropertyAsync(string collectionName,
            PutCollectionPropertyBody body, CancellationToken cancellationToken);

        /// <summary>
        /// Contains the number of documents and additional statistical information about the collection.
        /// GET/_api/collection/{collection-name}/figures
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<GetCollectionFiguresResponse> GetCollectionFiguresAsync(string collectionName,
            CancellationToken cancellationToken);
    }
}
