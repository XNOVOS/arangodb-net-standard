﻿using System.Threading;
using ArangoDBNetStandard.GraphApi.Models;
using System.Threading.Tasks;

namespace ArangoDBNetStandard.GraphApi
{
    /// <summary>
    /// Defines a client to access the ArangoDB Graph API.
    /// </summary>
    public interface IGraphApiClient
    {
        /// <summary>
        /// Creates a new graph in the graph module.
        /// POST /_api/gharial
        /// </summary>
        /// <param name="postGraphBody">The information of the graph to create.</param>
        /// <returns></returns>
        Task<PostGraphResponse> PostGraphAsync(
          PostGraphBody postGraphBody,
          PostGraphQuery query = null,
          CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists all graphs stored in this database.
        /// GET /_api/gharial
        /// </summary>
        /// <remarks>
        /// Note: The <see cref="GraphResult.Name"/> property is null for <see cref="GraphApiClient.GetGraphsAsync"/>
        /// in ArangoDB 4.5.2 and below, in which case you can use <see cref="GraphResult._key"/> instead.
        /// </remarks>
        /// <returns></returns>
        Task<GetGraphsResponse> GetGraphsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an existing graph object by name.
        /// Optionally all collections not used by other
        /// graphs can be deleted as well, using <see cref = "DeleteGraphQuery" ></ see >.
        /// DELETE /_api/gharial/{graph-name}
        /// </summary>
        /// <param name="graphName"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Task<DeleteGraphResponse> DeleteGraphAsync(
          string graphName,
          DeleteGraphQuery query = null);

        /// <summary>
        /// Selects information for a given graph.
        /// Will return the edge definitions as well as the orphan collections.
        /// GET /_api/gharial/{graph}
        /// </summary>
        /// <param name="graphName"></param>
        /// <returns></returns>
        Task<GetGraphResponse> GetGraphAsync(string graphName);

        /// <summary>
        /// Lists all vertex collections within the given graph.
        /// GET /_api/gharial/{graph}/vertex
        /// </summary>
        /// <param name="graph">The name of the graph.</param>
        /// <returns></returns>
        Task<GetVertexCollectionsResponse> GetVertexCollectionsAsync(string graphName);

        /// <summary>
        /// Lists all edge collections within this graph.
        /// GET /_api/gharial/{graph}/edge
        /// </summary>
        /// <param name="graphName"></param>
        /// <returns></returns>
        Task<GetEdgeCollectionsResponse> GetEdgeCollectionsAsync(string graphName);

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
        Task<PostEdgeDefinitionResponse> PostEdgeDefinitionAsync(
          string graphName,
          PostEdgeDefinitionBody body);

        /// <summary>
        /// Adds a vertex collection to the set of orphan collections of the graph.
        /// If the collection does not exist, it will be created.
        /// POST /_api/gharial/{graph}/vertex
        /// </summary>
        /// <param name="graphName">The name of the graph.</param>
        /// <param name="body">The information of the vertex collection.</param>
        /// <returns></returns>
        Task<PostVertexCollectionResponse> PostVertexCollectionAsync(
          string graphName,
          PostVertexCollectionBody body);

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
        Task<PostVertexResponse<T>> PostVertexAsync<T>(
          string graphName,
          string collectionName,
          T vertex,
          PostVertexQuery query = null);

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
        Task<DeleteEdgeDefinitionResponse> DeleteEdgeDefinitionAsync(
          string graphName,
          string collectionName,
          DeleteEdgeDefinitionQuery query = null,
          CancellationToken cancellationToken = default);

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
        Task<DeleteVertexCollectionResponse> DeleteVertexCollectionAsync(
          string graphName,
          string collectionName,
          DeleteVertexCollectionQuery query = null);

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
        Task<PostEdgeResponse<T>> PostEdgeAsync<T>(
          string graphName,
          string collectionName,
          T edge,
          PostEdgeQuery query = null);

        /// <summary>
        /// Gets an edge from the given graph using the edge collection and _key attribute.
        /// </summary>
        /// <typeparam name="T">The type of the edge document to deserialize to.</typeparam>
        /// <param name="graphName">The name of the graph.</param>
        /// <param name="collectionName">The name of the edge collection the edge belongs to.</param>
        /// <param name="edgeKey">The _key attribute of the edge.</param>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<GetEdgeResponse<T>> GetEdgeAsync<T>(
           string graphName,
           string collectionName,
           string edgeKey,
           GetEdgeQuery query = null);

        /// <summary>
        /// Gets an edge from the given graph using the edge's document-handle.
        /// GET /_api/gharial/{graph}/edge/{collection}/{edge}
        /// </summary>
        /// <typeparam name="T">The type of the edge document to deserialize to.</typeparam>
        /// <param name="graphName">The name of the graph.</param>
        /// <param name="edgeHandle">The document-handle of the edge document.</param>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<GetEdgeResponse<T>> GetEdgeAsync<T>(
          string graphName,
          string edgeHandle,
          GetEdgeQuery query = null);

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
        Task<DeleteEdgeResponse<T>> DeleteEdgeAsync<T>(
          string graphName,
          string collectionName,
          string edgeKey,
          DeleteEdgeQuery query = null);

        /// Gets a vertex from the given collection.
        /// GET/_api/gharial/{graph}/vertex/{collection}/{vertex}
        /// </summary>
        /// <param name="graphName"></param>
        /// <param name="collectionName"></param>
        /// <param name="vertexKey"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<GetVertexResponse<T>> GetVertexAsync<T>(
          string graphName,
          string collectionName,
          string vertexKey,
          GetVertexQuery query = null);

        /// <summary>
        /// Removes a vertex from the collection.
        /// DELETE/_api/gharial/{graph}/vertex/{collection}/{vertex}
        /// </summary>
        /// <param name="graphName"></param>
        /// <param name="collectionName"></param>
        /// <param name="vertexKey"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<DeleteVertexResponse<T>> DeleteVertexAsync<T>(
          string graphName,
          string collectionName,
          string vertexKey,
          DeleteVertexQuery query = null);

        /// <summary>
        /// Updates the data of the specific vertex in the collection.
        /// PATCH/_api/gharial/{graph}/vertex/{collection}/{vertex}
        /// </summary>
        /// <typeparam name="TPatch">Type of the patch object</typeparam>
        /// <typeparam name="TReturned">Type of the returned document, only applies when
        /// <see cref="PatchVertexQuery.ReturnNew"/> or <see cref="PatchVertexQuery.ReturnOld"/>
        /// are used.</typeparam>
        /// <param name="graphName"></param>
        /// <param name="collectionName"></param>
        /// <param name="vertexKey"></param>
        /// <param name="body"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PatchVertexResponse<TReturned>> PatchVertexAsync<TPatch, TReturned>(
          string graphName,
          string collectionName,
          string vertexKey,
          TPatch body,
          PatchVertexQuery query = null);

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
        Task<PutEdgeResponse<T>> PutEdgeAsync<T>(
          string graphName,
          string collectionName,
          string edgeKey,
          T edge,
          PutEdgeQuery query = null);

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
        Task<PutEdgeDefinitionResponse> PutEdgeDefinitionAsync(
          string graphName,
          string collectionName,
          PutEdgeDefinitionBody body,
          PutEdgeDefinitionQuery query = null);

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
        Task<PatchEdgeResponse<TPatch>> PatchEdgeAsync<TPatch, TReturned>(
          string graphName,
          string collectionName,
          string edgeKey,
          TReturned edge,
          PatchEdgeQuery query = null);

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
        Task<PutVertexResponse<T>> PutVertexAsync<T>(
          string graphName,
          string collectionName,
          string key,
          T vertex,
          PutVertexQuery query = null);
    }
}
