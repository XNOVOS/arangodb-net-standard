using ArangoDBNetStandard.GraphApi.Models;
using ArangoDBNetStandard.Serialization;
using ArangoDBNetStandard.Transport;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ArangoDBNetStandard.GraphApi
{
    public class GraphApiClient : ApiClientBase, IGraphApiClient
    {
        protected override string ApiRootPath => "_api/gharial";

        /// <summary>
        /// Create an instance of <see cref="GraphApiClient"/>
        /// using the provided transport layer and the default JSON serialization.
        /// </summary>
        /// <param name="transport"></param>
        public GraphApiClient(IApiClientTransport transport)
            : base(transport, new JsonNetApiClientSerialization())
        {
        }

        /// <summary>
        /// Create an instance of <see cref="GraphApiClient"/>
        /// using the provided transport and serialization layers.
        /// </summary>
        /// <param name="transport"></param>
        /// <param name="serializer"></param>
        public GraphApiClient(IApiClientTransport transport, IApiClientSerialization serializer)
            : base(transport, serializer)
        {
        }

        /// <summary>
        /// Creates a new graph in the graph module.
        /// POST /_api/gharial
        /// </summary>
        /// <param name="postGraphBody">The information of the graph to create.</param>
        /// <returns></returns>
        public async Task<PostGraphResponse> PostGraphAsync(
            PostGraphBody postGraphBody,
            PostGraphQuery query = null,
            CancellationToken cancellationToken = default)
        {
            return await PostRequestAsync(ApiRootPath, response => new PostGraphResponse(response), postGraphBody, query,
                cancellationToken);
        }

        /// <summary>
        /// Lists all graphs stored in this database.
        /// GET /_api/gharial
        /// </summary>
        /// <remarks>
        /// Note: The <see cref="GraphResult.Name"/> property is null for <see cref="GraphApiClient.GetGraphsAsync"/>
        /// in ArangoDB 4.5.2 and below, in which case you can use <see cref="GraphResult._key"/> instead.
        /// </remarks>
        /// <returns></returns>
        public async Task<GetGraphsResponse> GetGraphsAsync(CancellationToken cancellationToken = default)
        {
            return await GetRequestAsync(ApiRootPath, response => new GetGraphsResponse(response), null,
                cancellationToken);
        }

        /// <summary>
        /// Deletes an existing graph object by name.
        /// Optionally all collections not used by other
        /// graphs can be deleted as well, using <see cref = "DeleteGraphOptions" ></ see >.
        /// DELETE /_api/gharial/{graph-name}
        /// </summary>
        /// <param name="graphName"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<DeleteGraphResponse> DeleteGraphAsync(string graphName, DeleteGraphOptions query = null,
            CancellationToken cancellationToken = default)
        {
            return await DeleteRequestAsync($"{ApiRootPath}/{WebUtility.UrlEncode(graphName)}",
                response => new DeleteGraphResponse(response), query, cancellationToken);
        }

        /// <summary>
        /// Selects information for a given graph.
        /// Will return the edge definitions as well as the orphan collections.
        /// GET /_api/gharial/{graph}
        /// </summary>
        /// <param name="graphName"></param>
        /// <returns></returns>
        public async Task<GetGraphResponse> GetGraphAsync(string graphName, CancellationToken cancellationToken = default)
        {
            return await GetRequestAsync($"{ApiRootPath}/{WebUtility.UrlEncode(graphName)}",
                response => new GetGraphResponse(response), null, cancellationToken);
        }

        /// <summary>
        /// Lists all vertex collections within the given graph.
        /// GET /_api/gharial/{graph}/vertex
        /// </summary>
        /// <param name="graph">The name of the graph.</param>
        /// <returns></returns>
        public async Task<GetVertexCollectionsResponse> GetVertexCollectionsAsync(string graphName, CancellationToken cancellationToken = default)
        {
            return await GetRequestAsync($"{ApiRootPath}{'/'}{WebUtility.UrlEncode(graphName)}/vertex",
                response => new GetVertexCollectionsResponse(response), null, cancellationToken);
        }

        /// <summary>
        /// Lists all edge collections within this graph.
        /// GET /_api/gharial/{graph}/edge
        /// </summary>
        /// <param name="graphName"></param>
        /// <returns></returns>
        public async Task<GetEdgeCollectionsResponse> GetEdgeCollectionsAsync(string graphName, CancellationToken cancellationToken = default)
        {
            return await GetRequestAsync($"{ApiRootPath}{'/'}{WebUtility.UrlEncode(graphName)}/edge",
                response => new GetEdgeCollectionsResponse(response), null, cancellationToken);
        }

        /// <summary>
        /// Adds an additional edge definition to the graph.
        /// This edge definition has to contain a collection and an array of
        /// each from and to vertex collections. An edge definition can only
        /// be added if this definition is either not used in any other graph, or
        /// it is used with exactly the same definition. It is not possible to
        /// store a definition “e” from “v1” to “v2” in the one graph, and “e”
        /// from “v2” to “v1” in the other graph.
        /// POST /_api/gharial/{graph}/edge
        /// </summary>
        /// <param name="graphName">The name of the graph.</param>
        /// <param name="body">The information of the edge definition.</param>
        /// <returns></returns>
        public async Task<PostEdgeDefinitionResponse> PostEdgeDefinitionAsync(
            string graphName,
            PostEdgeDefinitionBody body,
            CancellationToken cancellationToken = default)
        {
            return await PostRequestAsync($"{ApiRootPath}/{WebUtility.UrlEncode(graphName)}/edge",
                response => new PostEdgeDefinitionResponse(response), body, null, cancellationToken);
        }

        /// <summary>
        /// Adds a vertex collection to the set of orphan collections of the graph.
        /// If the collection does not exist, it will be created.
        /// POST /_api/gharial/{graph}/vertex
        /// </summary>
        /// <param name="graphName">The name of the graph.</param>
        /// <param name="body">The information of the vertex collection.</param>
        /// <returns></returns>
        public async Task<PostVertexCollectionResponse> PostVertexCollectionAsync(
            string graphName,
            PostVertexCollectionBody body,
            CancellationToken cancellationToken = default)
        {
            return await PostRequestAsync($"{ApiRootPath}{'/'}{WebUtility.UrlEncode(graphName)}/vertex",
                response => new PostVertexCollectionResponse(response), body, null, cancellationToken);
        }

        /// <summary>
        /// Adds a vertex to the given collection.
        /// POST/_api/gharial/{graph}/vertex/{collection}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="graphName"></param>
        /// <param name="collectionName"></param>
        /// <param name="vertex"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PostVertexResponse<T>> PostVertexAsync<T>(
            string graphName,
            string collectionName,
            T vertex,
            PostVertexOptions query = null,
            CancellationToken cancellationToken = default)
        {
            return await PostRequestAsync(
                $"{ApiRootPath}{'/'}{WebUtility.UrlEncode(graphName)}/vertex/{WebUtility.UrlEncode(collectionName)}",
                response => new PostVertexResponse<T>(response), vertex, query, cancellationToken);
        }

        /// <summary>
        /// Remove one edge definition from the graph. This will only remove the
        /// edge collection, the vertex collections remain untouched and can still
        /// be used in your queries.
        /// DELETE/_api/gharial/{graph}/edge/{definition}
        /// </summary>
        /// <param name="graphName"></param>
        /// <param name="collectionName"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<DeleteEdgeDefinitionResponse> DeleteEdgeDefinitionAsync(
            string graphName,
            string collectionName,
            DeleteEdgeDefinitionOptions query = null,
            CancellationToken cancellationToken = default)
        {
            return await DeleteRequestAsync(
                $"{ApiRootPath}/{WebUtility.UrlEncode(graphName)}/edge/{WebUtility.UrlEncode(collectionName)}",
                response => new DeleteEdgeDefinitionResponse(response), query, cancellationToken);
        }

        /// <summary>
        /// Removes a vertex collection from the graph and optionally deletes the collection,
        /// if it is not used in any other graph.
        /// It can only remove vertex collections that are no longer part of edge definitions,
        /// if they are used in edge definitions you are required to modify those first.
        /// DELETE/_api/gharial/{graph}/vertex/{collection}
        /// </summary>
        /// <param name="graphName"></param>
        /// <param name="collectionName"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<DeleteVertexCollectionResponse> DeleteVertexCollectionAsync(
            string graphName,
            string collectionName,
            DeleteVertexCollectionOptions query = null,
            CancellationToken cancellationToken = default)
        {
            return await DeleteRequestAsync(
                $"{ApiRootPath}/{WebUtility.UrlEncode(graphName)}/vertex/{WebUtility.UrlEncode(collectionName)}",
                response => new DeleteVertexCollectionResponse(response), query, cancellationToken);
        }

        /// <summary>
        /// Creates an edge in an existing graph.
        /// The edge must contain a _from and _to value
        /// referencing valid vertices in the graph.
        /// The edge has to conform to the definition of the edge collection it is added to.
        /// POST /_api/gharial/{graph}/edge/{collection}
        /// </summary>
        /// <typeparam name="T">The type of the edge to create.
        /// Must contain valid _from and _to properties once serialized.
        /// <c>null</c> properties are preserved during serialization.</typeparam>
        /// <param name="graphName">The name of the graph.</param>
        /// <param name="collectionName">The name of the edge collection the edge belongs to.</param>
        /// <param name="edge">The edge to create.</param>
        /// <returns></returns>
        public async Task<PostEdgeResponse<T>> PostEdgeAsync<T>(
            string graphName,
            string collectionName,
            T edge,
            PostEdgeQuery query = null, 
            CancellationToken cancellationToken = default)
        {
            return await PostRequestAsync(
                $"{ApiRootPath}/{WebUtility.UrlEncode(graphName)}/edge/{WebUtility.UrlEncode(collectionName)}",
                response => new PostEdgeResponse<T>(response), edge, query, cancellationToken);
        }

        /// <summary>
        /// Gets an edge from the given graph using the edge collection and _key attribute.
        /// </summary>
        /// <typeparam name="T">The type of the edge document to deserialize to.</typeparam>
        /// <param name="graphName">The name of the graph.</param>
        /// <param name="collectionName">The name of the edge collection the edge belongs to.</param>
        /// <param name="edgeKey">The _key attribute of the edge.</param>
        /// <param name="query"></param>
        /// <returns></returns>
        public Task<GetEdgeResponse<T>> GetEdgeAsync<T>(
            string graphName,
            string collectionName,
            string edgeKey,
            GetEdgeQuery query = null, 
            CancellationToken cancellationToken = default)
        {
            return GetEdgeAsync<T>(
                graphName,
                WebUtility.UrlEncode(collectionName) + '/' + WebUtility.UrlEncode(edgeKey),
                query, cancellationToken);
        }

        /// <summary>
        /// Gets an edge from the given graph using the edge's document-handle.
        /// GET /_api/gharial/{graph}/edge/{collection}/{edge}
        /// </summary>
        /// <typeparam name="T">The type of the edge document to deserialize to.</typeparam>
        /// <param name="graphName">The name of the graph.</param>
        /// <param name="edgeHandle">The document-handle of the edge document.</param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<GetEdgeResponse<T>> GetEdgeAsync<T>(
            string graphName,
            string edgeHandle,
            GetEdgeQuery query = null,
            CancellationToken cancellationToken = default)
        {
            return await GetRequestAsync($"{ApiRootPath}/{WebUtility.UrlEncode(graphName)}/edge/{edgeHandle}",
                response => new GetEdgeResponse<T>(response), query, cancellationToken);
        }

        /// <summary>
        /// Removes an edge from the collection.
        /// DELETE /_api/gharial/{graph}/edge/{collection}/{edge}
        /// </summary>
        /// <typeparam name="T">The type of the edge that is returned in
        /// <see cref="DeleteEdgeResponse{T}.Old"/> if requested.</typeparam>
        /// <param name="graphName">The name of the graph.</param>
        /// <param name="collectionName">The name of the edge collection the edge belongs to.</param>
        /// <param name="edgeKey">The _key attribute of the edge.</param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<DeleteEdgeResponse<T>> DeleteEdgeAsync<T>(
            string graphName,
            string collectionName,
            string edgeKey,
            DeleteEdgeQuery query = null,
            CancellationToken cancellationToken = default)
        {
            return await DeleteRequestAsync(
                $"{ApiRootPath}/{WebUtility.UrlEncode(graphName)}/edge/{WebUtility.UrlEncode(collectionName)}/{WebUtility.UrlEncode(edgeKey)}",
                response => new DeleteEdgeResponse<T>(response), query, cancellationToken);
        }

        /// Gets a vertex from the given collection.
        /// GET/_api/gharial/{graph}/vertex/{collection}/{vertex}
        /// </summary>
        /// <param name="graphName"></param>
        /// <param name="collectionName"></param>
        /// <param name="vertexKey"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<GetVertexResponse<T>> GetVertexAsync<T>(
            string graphName,
            string collectionName,
            string vertexKey,
            GetVertexQuery query = null,
            CancellationToken cancellationToken = default)
        {
            return await GetRequestAsync(ApiRootPath + '/' + WebUtility.UrlEncode(graphName) +
                                         "/vertex/" + WebUtility.UrlEncode(collectionName) + "/" + vertexKey,
                response => new GetVertexResponse<T>(response), query, cancellationToken);
        }

        /// <summary>
        /// Removes a vertex from the collection.
        /// DELETE/_api/gharial/{graph}/vertex/{collection}/{vertex}
        /// </summary>
        /// <param name="graphName"></param>
        /// <param name="collectionName"></param>
        /// <param name="vertexKey"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<DeleteVertexResponse<T>> DeleteVertexAsync<T>(
            string graphName,
            string collectionName,
            string vertexKey,
            DeleteVertexQuery query = null,
            CancellationToken cancellationToken = default)
        {
            return await DeleteRequestAsync(
                $"{ApiRootPath}{'/'}{WebUtility.UrlEncode(graphName)}/vertex/{WebUtility.UrlEncode(collectionName)}/{WebUtility.UrlEncode(vertexKey)}",
                response => new DeleteVertexResponse<T>(response), query, cancellationToken);
        }

        /// <summary>
        /// Updates the data of the specific vertex in the collection.
        /// PATCH/_api/gharial/{graph}/vertex/{collection}/{vertex}
        /// </summary>
        /// <typeparam name="T">Type of the patch object</typeparam>
        /// <typeparam name="TReturned">Type of the returned document, only applies when
        /// <see cref="PatchVertexQuery.ReturnNew"/> or <see cref="PatchVertexQuery.ReturnOld"/>
        /// are used.</typeparam>
        /// <param name="graphName"></param>
        /// <param name="collectionName"></param>
        /// <param name="vertexKey"></param>
        /// <param name="body"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PatchVertexResponse<TReturned>> PatchVertexAsync<TPatch, TReturned>(
            string graphName,
            string collectionName,
            string vertexKey,
            TPatch body,
            PatchVertexQuery query = null,
            CancellationToken cancellationToken = default)
        {
            return await PatchRequestAsync(ApiRootPath + '/' + WebUtility.UrlEncode(graphName) +
                                           "/vertex/" + WebUtility.UrlEncode(collectionName) + "/" +
                                           WebUtility.UrlEncode(vertexKey),
                response => new PatchVertexResponse<TReturned>(response), body, query, cancellationToken);
        }

        /// <summary>
        /// Replaces the data of an edge in the collection.
        /// PUT /_api/gharial/{graph}/edge/{collection}/{edge}
        /// </summary>
        /// <typeparam name="T">Type of the document used for the update.</typeparam>
        /// <param name="graphName"></param>
        /// <param name="collectionName"></param>
        /// <param name="edgeKey"></param>
        /// <param name="edge"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PutEdgeResponse<T>> PutEdgeAsync<T>(
            string graphName,
            string collectionName,
            string edgeKey,
            T edge,
            PutEdgeQuery query = null,
            CancellationToken cancellationToken = default)
        {
            return await PutRequestAsync(
                $"{ApiRootPath}/{WebUtility.UrlEncode(graphName)}/edge/{WebUtility.UrlEncode(collectionName)}/{WebUtility.UrlEncode(edgeKey)}",
                response => new PutEdgeResponse<T>(response), edge, query, cancellationToken);
        }

        /// <summary>
        /// Change one specific edge definition.
        /// This will modify all occurrences of this definition in all graphs known to your database.
        /// PUT/_api/gharial/{graph}/edge/{definition}
        /// </summary>
        /// <param name="graphName"></param>
        /// <param name="collectionName"></param>
        /// <param name="body"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PutEdgeDefinitionResponse> PutEdgeDefinitionAsync(
            string graphName,
            string collectionName,
            PutEdgeDefinitionBody body,
            PutEdgeDefinitionQuery query = null,
            CancellationToken cancellationToken = default)
        {
            return await PutRequestAsync(ApiRootPath + "/" +
                                         WebUtility.UrlEncode(graphName) + "/edge/" +
                                         WebUtility.UrlEncode(collectionName),
                response => new PutEdgeDefinitionResponse(response), body, query, cancellationToken);
        }

        /// <summary>
        /// Updates the data of the specific edge in the collection.
        /// PATCH/_api/gharial/{graph}/edge/{collection}/{edge}
        /// </summary>
        /// <typeparam name="TPatch">Type of the returned edge document, when ReturnOld or ReturnNew query params are used.</typeparam>
        /// <typeparam name="TReturned">Type of the patch object used to perform a partial update of the edge document.</typeparam>
        /// <param name="graphName"></param>
        /// <param name="collectionName"></param>
        /// <param name="edgeKey"></param>
        /// <param name="edge"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PatchEdgeResponse<TReturned>> PatchEdgeAsync<TPatch, TReturned>(
            string graphName,
            string collectionName,
            string edgeKey,
            TPatch edge,
            PatchEdgeQuery query = null,
            CancellationToken cancellationToken = default)
        {
            return await PatchRequestAsync(
                $"{ApiRootPath}/{WebUtility.UrlEncode(graphName)}/edge/{WebUtility.UrlEncode(collectionName)}/{WebUtility.UrlEncode(edgeKey)}",
                response => new PatchEdgeResponse<TReturned>(response), edge, query, cancellationToken);
        }

        /// <summary>
        /// Replaces the data of a vertex in the collection.
        /// PUT/_api/gharial/{graph}/vertex/{collection}/{vertex}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="graphName"></param>
        /// <param name="collectionName"></param>
        /// <param name="key"></param>
        /// <param name="vertex"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PutVertexResponse<T>> PutVertexAsync<T>(
            string graphName,
            string collectionName,
            string key,
            T vertex,
            PutVertexQuery query = null,
            CancellationToken cancellationToken = default)
        {
            return await PutRequestAsync(
                $"{ApiRootPath}/{WebUtility.UrlEncode(graphName)}/vertex/{WebUtility.UrlEncode(collectionName)}/{WebUtility.UrlEncode(key)}",
                response => new PutVertexResponse<T>(response), vertex, query, cancellationToken);
        }
    }
}
