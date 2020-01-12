using ArangoDBNetStandard.CollectionApi.Models;
using ArangoDBNetStandard.Serialization;
using ArangoDBNetStandard.Transport;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ArangoDBNetStandard.CollectionApi
{
    /// <summary>
    /// A client for interacting with ArangoDB Collections endpoints,
    /// implementing <see cref="ICollectionApiClient"/>.
    /// </summary>
    public class CollectionApiClient : ApiClientBase, ICollectionApiClient
    {
        protected override string ApiRootPath => "_api/collection";

        /// <summary>
        /// Creates an instance of <see cref="CollectionApiClient"/>
        /// using the provided transport layer and the default JSON serialization.
        /// </summary>
        /// <param name="client"></param>
        public CollectionApiClient(IApiClientTransport transport)
            : base(transport, new JsonNetApiClientSerialization())
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="CollectionApiClient"/>
        /// using the provided transport and serialization layers.
        /// </summary>
        /// <param name="transport"></param>
        /// <param name="serializer"></param>
        public CollectionApiClient(IApiClientTransport transport, IApiClientSerialization serializer)
            : base(transport, serializer)
        {
        }

        public async Task<PostCollectionResponse> PostCollectionAsync(PostCollectionBody body,
            PostCollectionOptions options = null, CancellationToken cancellationToken = default)
        {
            return await PostRequestAsync(
                ApiRootPath,
                errorResponse => new PostCollectionResponse(errorResponse),
                body, options, cancellationToken);
        }

        public async Task<DeleteCollectionResponse> DeleteCollectionAsync(string collectionName,
            CancellationToken cancellationToken = default)
        {
            return await DeleteRequestAsync(ApiRootPath + "/" + WebUtility.UrlEncode(collectionName),
                errorResponse => new DeleteCollectionResponse(errorResponse), null, cancellationToken);
        }

        /// <summary>
        /// Truncates a collection, i.e. removes all documents in the collection.
        /// PUT/_api/collection/{collection-name}/truncate
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public async Task<TruncateCollectionResponse> TruncateCollectionAsync(string collectionName,
            CancellationToken cancellationToken = default)
        {
            return await PutRequestAsync(ApiRootPath + "/" + WebUtility.UrlEncode(collectionName) + "/truncate",
                errorResponse => new TruncateCollectionResponse(errorResponse), null, null, cancellationToken);
        }

        /// <summary>
        /// Gets count of documents in a collection.
        /// GET/_api/collection/{collection-name}/count
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public async Task<GetCollectionCountResponse> GetCollectionCountAsync(string collectionName,
            CancellationToken cancellationToken = default)
        {
            return await GetRequestAsync($"{ApiRootPath}/{WebUtility.UrlEncode(collectionName)}/count",
                response => new GetCollectionCountResponse(response), null, cancellationToken);
        }

        /// <summary>
        /// Get all collections.
        /// GET/_api/collection
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<GetCollectionsResponse> GetCollectionsAsync(GetCollectionsOptions query = null,
            CancellationToken cancellationToken = default)
        {
            return await GetRequestAsync(ApiRootPath, response => new GetCollectionsResponse(response), query,
                cancellationToken);
        }

        /// <summary>
        /// Gets the requested collection.
        /// GET/_api/collection/{collection-name}
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public async Task<GetCollectionResponse> GetCollectionAsync(string collectionName, CancellationToken cancellationToken = default)
        {
            return await GetRequestAsync(
                ApiRootPath + "/" + WebUtility.UrlEncode(collectionName),
                errorResponse => new GetCollectionResponse(errorResponse), cancellationToken: cancellationToken);
        }

        /// Read properties of a collection.
        /// GET /_api/collection/{collection-name}/properties
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<GetCollectionPropertiesResponse> GetCollectionPropertiesAsync(string collectionName,
            CancellationToken cancellationToken = default)
        {
            return await GetRequestAsync($"{ApiRootPath}/{WebUtility.UrlEncode(collectionName)}/properties",
                response => new GetCollectionPropertiesResponse(response), null, cancellationToken);
        }

        /// <summary>
        /// Rename a collection.
        /// PUT /_api/collection/{collection-name}/rename
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="body"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RenameCollectionResponse> RenameCollectionAsync(string collectionName,
            RenameCollectionBody body, CancellationToken cancellationToken = default)
        {
            return await PutRequestAsync($"{ApiRootPath}/{WebUtility.UrlEncode(collectionName)}/rename",
                response => new RenameCollectionResponse(response), body, null, cancellationToken);
        }

        /// <summary>
        /// Get a revision of the collection. 
        /// GET /_api/collection/{collection-name}/revision
        /// </summary>
        /// <param name="collectionName">Name of the collection</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<GetCollectionRevisionResponse> GetCollectionRevisionAsync(string collectionName,
            CancellationToken cancellationToken = default)
        {
            return await GetRequestAsync($"{ApiRootPath}/{WebUtility.UrlEncode(collectionName)}/revision",
                response => new GetCollectionRevisionResponse(response), null, cancellationToken);
        }

        /// <summary>
        /// Changes the properties of a collection
        /// PUT /_api/collection/{collection-name}/properties
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="body"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PutCollectionPropertyResponse> PutCollectionPropertyAsync(string collectionName,
            PutCollectionPropertyBody body, CancellationToken cancellationToken = default)
        {
            return await PutRequestAsync($"{ApiRootPath}/{collectionName}/properties",
                response => new PutCollectionPropertyResponse(response), body, null, cancellationToken);
        }

        /// <summary>
        /// Contains the number of documents and additional statistical information about the collection.
        /// GET/_api/collection/{collection-name}/figures
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<GetCollectionFiguresResponse> GetCollectionFiguresAsync(string collectionName,
            CancellationToken cancellationToken = default)
        {
            return await GetRequestAsync($"{ApiRootPath}/{WebUtility.UrlEncode(collectionName)}/figures",
                response => new GetCollectionFiguresResponse(response), null, cancellationToken);
        }
    }
}
