﻿using ArangoDBNetStandard;
using ArangoDBNetStandard.CollectionApi.Models;
using ArangoDBNetStandard.DocumentApi.Models;
using ArangoDBNetStandard.GraphApi;
using ArangoDBNetStandard.GraphApi.Models;
using ArangoDBNetStandardTest.GraphApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ArangoDBNetStandardTest.GraphApi
{
    public class GraphApiClientTest : IClassFixture<GraphApiClientTestFixture>
    {
        private readonly GraphApiClientTestFixture _fixture;
        private readonly GraphApiClient _client;

        public GraphApiClientTest(GraphApiClientTestFixture fixture)
        {
            _fixture = fixture;
            _client = fixture.ArangoDBClient.Graph;
            _client.ThrowErrorsAsExceptions = false;
        }

        [Fact]
        public async Task GetGraphsAsync_ShouldSucceed()
        {
            // get the list of graphs
            var graphsResult = await _fixture.ArangoDBClient.Graph.GetGraphsAsync();

            // test result
            Assert.Equal(HttpStatusCode.OK, graphsResult.ResponseDetails.Code);
            Assert.NotEmpty(graphsResult);

            var graph = graphsResult.First(x => x._key == _fixture.TestGraph);
            Assert.NotEmpty(graph.EdgeDefinitions);
            Assert.Empty(graph.OrphanCollections);
            Assert.Equal(1, graph.NumberOfShards);
            Assert.Equal(1, graph.ReplicationFactor);
            Assert.False(graph.IsSmart);
            Assert.Equal(_fixture.TestGraph, graph._key);
            Assert.Equal("_graphs/" + _fixture.TestGraph, graph._id);
            Assert.NotNull(graph._rev);
        }

        [Fact]
        public async Task DeleteGraphAsync_ShouldSucceed()
        {
            await _fixture.ArangoDBClient.Graph.PostGraphAsync(new PostGraphBody
            {
                Name = "temp_graph",
                EdgeDefinitions = new List<EdgeDefinition>
                {
                    new EdgeDefinition
                    {
                        From = new[] { "fromclx" },
                        To = new[] { "toclx" },
                        Collection = "clx"
                    }
                }
            });
            var query = new DeleteGraphOptions
            {
                DropCollections = false
            };
            var response = await _client.DeleteGraphAsync("temp_graph", query);
            Assert.Equal(HttpStatusCode.Accepted, response.ResponseDetails.Code);
            Assert.True(response.Removed);
            Assert.False(response.ResponseDetails.Error);
        }

        [Fact]
        public async Task DeleteGraphAsync_ShouldThrow_WhenNotFound()
        {
            _client.ThrowErrorsAsExceptions = true;
            var exception = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _client.DeleteGraphAsync("boggus_graph", new DeleteGraphOptions
                {
                    DropCollections = false
                });
            });

            Assert.Equal(HttpStatusCode.NotFound, exception.ResponseDetails.Code);
            Assert.Equal(1924, exception.ResponseDetails.ErrorNum); // GRAPH_NOT_FOUND
        }
        [Fact]
        public async Task DeleteGraphAsync_ShouldReturnError_WhenNotFound()
        {
            DeleteGraphResponse deleteGraphResponse = await _client.DeleteGraphAsync("boggus_graph", new DeleteGraphOptions
            {
                DropCollections = false
            });

            Assert.False(deleteGraphResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, deleteGraphResponse.ResponseDetails.Code);
            Assert.Equal(1924, deleteGraphResponse.ResponseDetails.ErrorNum); // GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task GetGraphAsync_ShouldSucceed()
        {
            var response = await _client.GetGraphAsync(_fixture.TestGraph);
            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);
            Assert.Equal("_graphs/" + _fixture.TestGraph, response.Graph._id);
            Assert.NotEmpty(response.Graph.EdgeDefinitions);
            Assert.Empty(response.Graph.OrphanCollections);
            Assert.Equal(1, response.Graph.NumberOfShards);
            Assert.Equal(1, response.Graph.ReplicationFactor);
            Assert.False(response.Graph.IsSmart);
            Assert.Equal(_fixture.TestGraph, response.Graph._key);
            Assert.Equal("_graphs/" + _fixture.TestGraph, response.Graph._id);
            Assert.NotNull(response.Graph._rev);
        }

        [Fact]
        public async Task GetGraphAsync_ShouldThrow_WhenNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            var exception = await Assert.ThrowsAsync<ApiErrorException>(async () => await _client.GetGraphAsync("bogus_graph"));
            Assert.Equal(HttpStatusCode.NotFound, exception.ResponseDetails.Code);
            Assert.Equal(1924, exception.ResponseDetails.ErrorNum); // GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task GetVertexCollectionsAsync_ShouldSucceed()
        {
            // Create an edge collection

            string edgeClx = nameof(GetGraphsAsync_ShouldSucceed) + "_EdgeClx";

            var createClxResponse = await _fixture.ArangoDBClient.Collection.PostCollectionAsync(
                new PostCollectionBody
                {
                    Name = edgeClx,
                    Type = 3
                });

            Assert.Equal(edgeClx, createClxResponse.Name);

            // Create a Graph

            string graphName = nameof(GetVertexCollectionsAsync_ShouldSucceed);

            await _client.PostGraphAsync(new PostGraphBody
            {
                Name = graphName,
                EdgeDefinitions = new List<EdgeDefinition>
                {
                    new EdgeDefinition
                    {
                        Collection = edgeClx,
                        From = new[] { "FromCollection" },
                        To = new[] { "ToCollection" }
                    }
                }
            });

            // List the vertex collections

            GetVertexCollectionsResponse response = await _client.GetVertexCollectionsAsync(graphName);

            Assert.Equal(2, response.Count());
            Assert.Contains("FromCollection", response);
            Assert.Contains("ToCollection", response);
        }

        [Fact]
        public async Task GetVertexCollectionsAsync_ShouldThrow_WhenGraphDoesNotExist()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () => await _client.GetVertexCollectionsAsync("GraphThatDoesNotExist"));

            ApiResponse apiError = ex.ResponseDetails;

            Assert.Equal(HttpStatusCode.NotFound, apiError.Code);
            Assert.Equal(1924, apiError.ErrorNum);
        }

        [Fact]
        public async Task GetEdgeCollectionsAsync_ShouldSucceed()
        {
            var response = await _client.GetEdgeCollectionsAsync(_fixture.TestGraph);
            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);
            Assert.NotEmpty(response);
            Assert.False(response.ResponseDetails.Error);
        }

        [Fact]
        public async Task GetEdgeCollectionsAsync_ShouldThrow_WhenGraphIsNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            var exception = await Assert.ThrowsAsync<ApiErrorException>(async () => await _client.GetEdgeCollectionsAsync("bogus_graph"));
            Assert.Equal(HttpStatusCode.NotFound, exception.ResponseDetails.Code);
            Assert.Equal(1924, exception.ResponseDetails.ErrorNum); // GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task PostGraphAsync_ShouldSucceed()
        {
            var graphName = nameof(PostGraphAsync_ShouldSucceed) + "_graph";
            var response = await _client.PostGraphAsync(new PostGraphBody
            {
                Name = graphName,
                EdgeDefinitions = new List<EdgeDefinition>
                {
                    new EdgeDefinition
                    {
                        From = new[] { "fromclx" },
                        To = new[] { "toclx" },
                        Collection = "clx"
                    }
                },
                OrphanCollections = new List<string>
                {
                    "myclx"
                }
            });

            Assert.Equal(HttpStatusCode.Accepted, response.ResponseDetails.Code);
            Assert.Single(response.Graph.EdgeDefinitions);
            Assert.Contains(response.Graph.OrphanCollections, x => x == "myclx");
            Assert.Equal(graphName, response.Graph.Name);
        }

        [Fact]
        public async Task PostGraphAsync_ShouldSucceed_WhenWaitForSyncIsTrue()
        {
            string graphName = nameof(PostGraphAsync_ShouldSucceed_WhenWaitForSyncIsTrue);

            var response = await _client.PostGraphAsync(new PostGraphBody
            {
                Name = graphName,
                EdgeDefinitions = new List<EdgeDefinition>
                {
                    new EdgeDefinition
                    {
                        From = new[] { "fromclx" },
                        To = new[] { "toclx" },
                        Collection = "clx"
                    }
                }
            },
            new PostGraphQuery
            {
                WaitForSync = true
            });

            Assert.Equal(HttpStatusCode.Created, response.ResponseDetails.Code);
            Assert.Single(response.Graph.EdgeDefinitions);
            Assert.Equal(graphName, response.Graph.Name);
        }

        [Fact]
        public async Task PostGraphAsync_ShouldThrow_WhenGraphNameIsInvalid()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _client.PostGraphAsync(new PostGraphBody
                {
                    Name = "Bad Graph Name",
                    EdgeDefinitions = new List<EdgeDefinition>
                {
                    new EdgeDefinition
                    {
                        From = new[] { "fromclx" },
                        To = new[] { "toclx" },
                        Collection = "clx"
                    }
                }
                });
            });

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseDetails.Code);
            Assert.Equal(1221, ex.ResponseDetails.ErrorNum); // ARANGO_DOCUMENT_KEY_BAD
        }

        [Fact]
        public async Task PostEdgeDefinitionAsync_ShouldSucceed()
        {
            string tempGraph = nameof(PostEdgeDefinitionAsync_ShouldSucceed);
            await _client.PostGraphAsync(new PostGraphBody
            {
                Name = tempGraph,
                EdgeDefinitions = new List<EdgeDefinition>()
            });
            var response = await _client.PostEdgeDefinitionAsync(
                tempGraph,
                new PostEdgeDefinitionBody
                {
                    From = new[] { "fromclxx" },
                    To = new[] { "toclxx" },
                    Collection = "clxx"
                });
            Assert.Equal(HttpStatusCode.Accepted, response.ResponseDetails.Code);
            Assert.False(response.ResponseDetails.Error);
            Assert.Single(response.Graph.EdgeDefinitions);
            Assert.Equal(tempGraph, response.Graph.Name);
            Assert.Equal("_graphs/" + tempGraph, response.Graph._id);
            Assert.NotNull(response.Graph._rev);
            Assert.False(response.Graph.IsSmart);
            Assert.Empty(response.Graph.OrphanCollections);
        }

        [Fact]
        public async Task PostEdgeDefinitionAsync_ShouldThrow_WhenGraphNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            var exception = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _client.PostEdgeDefinitionAsync(
                    "boggus_graph",
                    new PostEdgeDefinitionBody
                    {
                        From = new[] { "fromclxx" },
                        To = new[] { "toclxx" },
                        Collection = "clxx"
                    });
            });

            Assert.Equal(HttpStatusCode.NotFound, exception.ResponseDetails.Code);
            Assert.Equal(1924, exception.ResponseDetails.ErrorNum); // GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task PostVertexCollectionAsync_ShouldSucceed()
        {
            // Create a new graph

            string graphName = nameof(PostVertexCollectionAsync_ShouldSucceed);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });

            // Add a vertex collection

            string clxToAdd = nameof(PostVertexCollectionAsync_ShouldSucceed);

            PostVertexCollectionResponse response = await _client.PostVertexCollectionAsync(
                graphName,
                new PostVertexCollectionBody
                {
                    Collection = clxToAdd
                });

            Assert.Equal(HttpStatusCode.Accepted, response.ResponseDetails.Code);
            Assert.False(response.ResponseDetails.Error);

            GraphResult graph = response.Graph;

            Assert.Contains(clxToAdd, graph.OrphanCollections);
        }

        [Fact]
        public async Task PostVertexCollectionAsync_ShouldThrow_WhenGraphIsNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            string graphName = nameof(PostVertexCollectionAsync_ShouldThrow_WhenGraphIsNotFound);

            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _client.PostVertexCollectionAsync(graphName, new PostVertexCollectionBody
                {
                    Collection = "VertexCollectionThatShouldNotBeCreated"
                });
            });

            ApiResponse apiError = ex.ResponseDetails;

            Assert.True(apiError.Error);
            Assert.Equal(HttpStatusCode.NotFound, apiError.Code);
            Assert.Equal(1924, apiError.ErrorNum); // ERROR_GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task PostVertexAsync_ShouldSucceed()
        {
            // Create a new graph

            string graphName = nameof(PostVertexAsync_ShouldSucceed);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });

            // Add a vertex collection

            string clxToAdd = nameof(PostVertexCollectionAsync_ShouldSucceed);

            await _client.PostVertexCollectionAsync(
                graphName,
                new PostVertexCollectionBody
                {
                    Collection = clxToAdd
                });

            var response = await _client.PostVertexAsync<object>(graphName, clxToAdd, new
            {
                Name = clxToAdd + "_vtx"
            });

            Assert.Equal(HttpStatusCode.Accepted, response.ResponseDetails.Code);
            Assert.NotNull(response.Vertex._id);
            Assert.False(response.ResponseDetails.Error);
            Assert.NotNull(response.Vertex);
        }

        [Fact]
        public async Task PostVertexAsync_ShouldThrow_WhenGraphIsNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            string graphName = nameof(PostVertexAsync_ShouldThrow_WhenGraphIsNotFound);
            string vertex = nameof(PostVertexAsync_ShouldThrow_WhenGraphIsNotFound) + "_vtx";

            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () => await _client.PostVertexAsync(graphName, vertex, new { }));

            Assert.True(ex.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1924, ex.ResponseDetails.ErrorNum); // ERROR_GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task PostVertexAsync_ShouldThrow_WhenVertexCollectionIsNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            // Create a new graph
            string graphName = nameof(PostVertexAsync_ShouldThrow_WhenVertexCollectionIsNotFound);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });
            string vertex = nameof(PostVertexAsync_ShouldThrow_WhenVertexCollectionIsNotFound) + "_vtx";

            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () => await _client.PostVertexAsync(graphName, vertex, new { }));

            Assert.True(ex.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1203, ex.ResponseDetails.ErrorNum); // ARANGO_DATA_SOURCE_NOT_FOUND
        }

        [Fact]
        public async Task PostVertexAsync_ShouldReturnNewVertex_WhenReturnNewIsTrue()
        {
            // Create a new graph

            string graphName = nameof(PostVertexAsync_ShouldReturnNewVertex_WhenReturnNewIsTrue);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });

            // Add a vertex collection

            string clxToAdd = nameof(PostVertexAsync_ShouldReturnNewVertex_WhenReturnNewIsTrue);

            await _client.PostVertexCollectionAsync(
                graphName,
                new PostVertexCollectionBody
                {
                    Collection = clxToAdd
                });
            var propertyName = clxToAdd + "_vtx";

            var response = await _client.PostVertexAsync(graphName, clxToAdd, new
            {
                Name = propertyName
            }, new PostVertexOptions
            {
                ReturnNew = true,
                WaitForSync = true
            });

            Assert.Equal(HttpStatusCode.Created, response.ResponseDetails.Code);
            Assert.False(response.ResponseDetails.Error);
            Assert.NotNull(response.New);
            Assert.Equal(propertyName, response.New.Name);
        }

        [Fact]
        public async Task DeleteEdgeDefinitionAsync_ShouldSucceed()
        {
            string edgeClx = nameof(DeleteEdgeDefinitionAsync_ShouldSucceed) + "_EdgeClx";

            var createClxResponse = await _fixture.ArangoDBClient.Collection.PostCollectionAsync(
                new PostCollectionBody
                {
                    Name = edgeClx,
                    Type = 3
                });

            Assert.Equal(edgeClx, createClxResponse.Name);

            string graphName = nameof(DeleteEdgeDefinitionAsync_ShouldSucceed);

            PostGraphResponse createGraphResponse = await _client.PostGraphAsync(new PostGraphBody
            {
                Name = graphName,
                EdgeDefinitions = new List<EdgeDefinition>
                {
                    new EdgeDefinition
                    {
                        Collection = edgeClx,
                        From = new[] { "FromPutCollection" },
                        To = new[] { "ToPutCollection" }
                    }
                }
            });

            Assert.Equal(HttpStatusCode.Accepted, createGraphResponse.ResponseDetails.Code);

            var response = await _client.DeleteEdgeDefinitionAsync(graphName, edgeClx, new DeleteEdgeDefinitionOptions
            {
                WaitForSync = false,
                DropCollections = true
            });

            Assert.Equal(HttpStatusCode.Accepted, response.ResponseDetails.Code);
            Assert.Empty(response.Graph.EdgeDefinitions);

            var getAfterResponse = await _client.GetEdgeCollectionsAsync(graphName);

            var collectionFound = getAfterResponse.FirstOrDefault(x => x == edgeClx);

            Assert.Null(collectionFound);
        }

        [Fact]
        public async Task DeleteEdgeDefinitionAsync_ShouldThrow_WhenEdgeDefinitionNameDoesNotExist()
        {
            _client.ThrowErrorsAsExceptions = true;
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _client.DeleteEdgeDefinitionAsync(_fixture.TestGraph, "bogus_edgeclx", new DeleteEdgeDefinitionOptions
                {
                    WaitForSync = false,
                    DropCollections = true
                });
            });

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1930, ex.ResponseDetails.ErrorNum); // GRAPH_EDGE_COLLECTION_NOT_USED
        }

        [Fact]
        public async Task DeleteEdgeDefinitionAsync_ShouldReturnError_WhenEdgeDefinitionNameDoesNotExist()
        {
            DeleteEdgeDefinitionResponse deleteEdgeDefinitionResponse = await _client.DeleteEdgeDefinitionAsync(_fixture.TestGraph, "bogus_edgeclx", new DeleteEdgeDefinitionOptions
            {
                WaitForSync = false,
                DropCollections = true
            });

            Assert.False(deleteEdgeDefinitionResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, deleteEdgeDefinitionResponse.ResponseDetails.Code);
            Assert.Equal(1930, deleteEdgeDefinitionResponse.ResponseDetails.ErrorNum); // GRAPH_EDGE_COLLECTION_NOT_USED
        }

        [Fact]
        public async Task DeleteEdgeDefinitionAsync_ShouldThrow_WhenGraphNameDoesNotExist()
        {
            _client.ThrowErrorsAsExceptions = true;
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _client.DeleteEdgeDefinitionAsync("bogus_graph", _fixture.TestCollection, new DeleteEdgeDefinitionOptions
                {
                    WaitForSync = false,
                    DropCollections = true
                });
            });

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1924, ex.ResponseDetails.ErrorNum); // GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task DeleteEdgeDefinitionAsync_ShouldReturnError_WhenGraphNameDoesNotExist()
        {
            DeleteEdgeDefinitionResponse deleteEdgeDefinitionResponse = await _client.DeleteEdgeDefinitionAsync("bogus_graph", _fixture.TestCollection, new DeleteEdgeDefinitionOptions
            {
                WaitForSync = false,
                DropCollections = true
            });

            Assert.Equal(HttpStatusCode.NotFound, deleteEdgeDefinitionResponse.ResponseDetails.Code);
            Assert.Equal(1924, deleteEdgeDefinitionResponse.ResponseDetails.ErrorNum); // GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task DeleteVertexCollectionAsync_ShouldSucceed()
        {
            // Create a new graph

            string graphName = nameof(DeleteVertexCollectionAsync_ShouldSucceed);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });

            // Add a vertex collection

            string clxToDelete = nameof(DeleteVertexCollectionAsync_ShouldSucceed);

            await _client.PostVertexCollectionAsync(
                graphName,
                new PostVertexCollectionBody
                {
                    Collection = clxToDelete
                });

            var response = await _client.DeleteVertexCollectionAsync(graphName, clxToDelete, new DeleteVertexCollectionOptions
            {
                DropCollection = false
            });

            Assert.Equal(HttpStatusCode.Accepted, response.ResponseDetails.Code);
            Assert.False(response.ResponseDetails.Error);
            Assert.Empty(response.Graph.OrphanCollections);
        }

        [Fact]
        public async Task DeleteVertexCollectionAsync_ShouldThrow_WhenGraphIsNotFound()
        {
            _client.ThrowErrorsAsExceptions = true;
            string graphName = nameof(DeleteVertexCollectionAsync_ShouldThrow_WhenGraphIsNotFound);
            string clxToDelete = nameof(DeleteVertexCollectionAsync_ShouldThrow_WhenGraphIsNotFound);

            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () => await _client.DeleteVertexCollectionAsync(graphName, clxToDelete));

            Assert.True(ex.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1924, ex.ResponseDetails.ErrorNum); // ERROR_GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task DeleteVertexCollectionAsync_ShouldReturnError_WhenGraphIsNotFound()
        {
            string graphName = nameof(DeleteVertexCollectionAsync_ShouldThrow_WhenGraphIsNotFound);
            string clxToDelete = nameof(DeleteVertexCollectionAsync_ShouldThrow_WhenGraphIsNotFound);

            DeleteVertexCollectionResponse deleteVertexCollectionResponse = await _client.DeleteVertexCollectionAsync(graphName, clxToDelete);
            
            Assert.False(deleteVertexCollectionResponse.IsSuccess);
            Assert.True(deleteVertexCollectionResponse.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, deleteVertexCollectionResponse.ResponseDetails.Code);
            Assert.Equal(1924, deleteVertexCollectionResponse.ResponseDetails.ErrorNum); // ERROR_GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task DeleteVertexCollectionAsync_ShouldThrow_WhenVertexIsNotFound()
        {
            _client.ThrowErrorsAsExceptions = true;
            string graphName = nameof(DeleteVertexCollectionAsync_ShouldThrow_WhenVertexIsNotFound);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });

            string clxToDelete = nameof(DeleteVertexCollectionAsync_ShouldThrow_WhenVertexIsNotFound);

            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () => await _client.DeleteVertexCollectionAsync(graphName, clxToDelete));

            Assert.True(ex.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseDetails.Code);
            Assert.Equal(1928, ex.ResponseDetails.ErrorNum); // GRAPH_NOT_IN_ORPHAN_COLLECTION
        }

        [Fact]
        public async Task DeleteVertexCollectionAsync_ShouldReturnError_WhenVertexIsNotFound()
        {
            string graphName = nameof(DeleteVertexCollectionAsync_ShouldThrow_WhenVertexIsNotFound);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });

            string clxToDelete = nameof(DeleteVertexCollectionAsync_ShouldThrow_WhenVertexIsNotFound);

            DeleteVertexCollectionResponse deleteVertexCollectionResponse = await _client.DeleteVertexCollectionAsync(graphName, clxToDelete);
            
            Assert.False(deleteVertexCollectionResponse.IsSuccess);
            Assert.True(deleteVertexCollectionResponse.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.BadRequest, deleteVertexCollectionResponse.ResponseDetails.Code);
            Assert.Equal(1928, deleteVertexCollectionResponse.ResponseDetails.ErrorNum); // GRAPH_NOT_IN_ORPHAN_COLLECTION
        }

        [Fact]
        public async Task DeleteVertexCollectionAsync_ShouldDropCollection_WhenDropCollectionIsTrue()
        {
            _client.ThrowErrorsAsExceptions = true;
            // Create a new graph

            string graphName = "DeleteVertexCollectionAsync_ShouldThrowNotFound_WhenCollectionDropIsTrue";

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });

            // Add a vertex collection

            string clxToDelete = "DeleteVertexCollectionAsync_ShouldDropCollection";

            await _client.PostVertexCollectionAsync(
                graphName,
                new PostVertexCollectionBody
                {
                    Collection = clxToDelete
                });

            await _client.DeleteVertexCollectionAsync(graphName, clxToDelete, new DeleteVertexCollectionOptions
            {
                DropCollection = true
            });

            GetCollectionResponse getCollectionResponse = await _fixture.ArangoDBClient.Collection.GetCollectionAsync(clxToDelete);
            
            Assert.False(getCollectionResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, getCollectionResponse.ResponseDetails.Code);
            Assert.Equal(1203, getCollectionResponse.ResponseDetails.ErrorNum); // ARANGO_DATA_SOURCE_NOT_FOUND
        }

        [Fact]
        public async Task PostEdgeAsync_ShouldSucceed()
        {
            string graphName = nameof(PostEdgeAsync_ShouldSucceed);
            string fromClx = graphName + "_fromclx";
            string toClx = graphName + "_toclx";
            string edgeClx = graphName + "_edgeclx";

            // Create a new graph

            await _fixture.ArangoDBClient.Graph.PostGraphAsync(new PostGraphBody
            {
                Name = graphName,
                EdgeDefinitions = new List<EdgeDefinition>
                {
                    new EdgeDefinition
                    {
                        From = new[] { fromClx },
                        To = new[] { toClx },
                        Collection = edgeClx
                    }
                }
            });

            // Create a document in the vertex collections

            PostDocumentResponse<object> fromResponse = await
                _fixture.ArangoDBClient.Document.PostDocumentAsync<object>(
                fromClx,
                new { myKey = "myValue" });

            PostDocumentResponse<object> toResponse = await
                _fixture.ArangoDBClient.Document.PostDocumentAsync<object>(
                toClx,
                new { myKey = "myValue" });

            // Create the edge

            var response = await _client.PostEdgeAsync(
                graphName,
                edgeClx,
                new
                {
                    _from = fromResponse._id,
                    _to = toResponse._id,
                    myKey = "myValue"
                },
                new PostEdgeQuery
                {
                    ReturnNew = true,
                    WaitForSync = true
                });

            Assert.Equal(HttpStatusCode.Created, response.ResponseDetails.Code);
            Assert.False(response.ResponseDetails.Error);
            Assert.NotNull(response.Edge);
            Assert.NotNull(response.Edge._id);
            Assert.NotNull(response.Edge._key);
            Assert.NotNull(response.Edge._rev);
            Assert.NotNull(response.New);
            Assert.Equal("myValue", response.New.myKey);
        }

        [Fact]
        public async Task PostEdgeAsync_ShouldThrow_WhenGraphNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            string graphName = nameof(PostEdgeAsync_ShouldThrow_WhenGraphNotFound);

            var exception = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _client.PostEdgeAsync(graphName, "edgeClx", new
                {
                    myKey = "myValue"
                });
            });

            Assert.Equal(HttpStatusCode.NotFound, exception.ResponseDetails.Code);
            Assert.Equal(1924, exception.ResponseDetails.ErrorNum); // GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task DeleteEdgeAsync_ShouldSucceed()
        {
            string graphName = nameof(DeleteEdgeAsync_ShouldSucceed);
            string fromClx = graphName + "_fromclx";
            string toClx = graphName + "_toclx";
            string edgeClx = graphName + "_edgeclx";

            // Create a new graph

            await _fixture.ArangoDBClient.Graph.PostGraphAsync(new PostGraphBody
            {
                Name = graphName,
                EdgeDefinitions = new List<EdgeDefinition>
                {
                    new EdgeDefinition
                    {
                        From = new[] { fromClx },
                        To = new[] { toClx },
                        Collection = edgeClx
                    }
                }
            });

            // Create a document in the vertex collections

            PostDocumentResponse<object> fromResponse = await
                _fixture.ArangoDBClient.Document.PostDocumentAsync<object>(
                fromClx,
                new { myKey = "myValue" });

            PostDocumentResponse<object> toResponse = await
                _fixture.ArangoDBClient.Document.PostDocumentAsync<object>(
                toClx,
                new { myKey = "myValue" });

            // Create the edge

            var createEdgeResponse = await _client.PostEdgeAsync(
                graphName,
                edgeClx,
                new
                {
                    _from = fromResponse._id,
                    _to = toResponse._id,
                    myKey = "myValue"
                },
                new PostEdgeQuery
                {
                    ReturnNew = true,
                    WaitForSync = true
                });
            // Delete edge
            DeleteEdgeResponse<DeleteGraphEdgeMockModel> response =
                await _client.DeleteEdgeAsync<DeleteGraphEdgeMockModel>(
                    graphName,
                    edgeClx,
                    createEdgeResponse.Edge._key,
                    new DeleteEdgeQuery
                    {
                        ReturnOld = true,
                        WaitForSync = true
                    });

            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);
            Assert.Equal(createEdgeResponse.New.myKey, response.Old.myKey);
            Assert.True(response.Removed);
            Assert.False(response.ResponseDetails.Error);
        }

        [Fact]
        public async Task DeleteEdgeAsync_ShouldThrow_WhenGraphNotFound()
        {
            _client.ThrowErrorsAsExceptions = true;
            string graphName = nameof(DeleteEdgeAsync_ShouldThrow_WhenGraphNotFound);

            var exception = await Assert.ThrowsAsync<ApiErrorException>(async () =>
                await _client.DeleteEdgeAsync<object>(graphName, "edgeClx", ""));

            Assert.Equal(HttpStatusCode.NotFound, exception.ResponseDetails.Code);
            Assert.Equal(1924, exception.ResponseDetails.ErrorNum); // GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task DeleteEdgeAsync_ShouldReturnError_WhenGraphNotFound()
        {
            string graphName = nameof(DeleteEdgeAsync_ShouldThrow_WhenGraphNotFound);

            DeleteEdgeResponse<object> deleteEdgeResponse = await _client.DeleteEdgeAsync<object>(graphName, "edgeClx", "");

            Assert.False(deleteEdgeResponse.IsSuccess);
            Assert.True(deleteEdgeResponse.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, deleteEdgeResponse.ResponseDetails.Code);
            Assert.Equal(1924, deleteEdgeResponse.ResponseDetails.ErrorNum); // GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task GetVertexAsync_ShouldSucceed()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            // Create a new graph

            string graphName = nameof(GetVertexAsync_ShouldSucceed);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });

            // Add a vertex collection

            string clxToAdd = nameof(GetVertexAsync_ShouldSucceed);

            await _client.PostVertexCollectionAsync(
                graphName,
                new PostVertexCollectionBody
                {
                    Collection = clxToAdd
                });

            var createVtxResponse = await _client.PostVertexAsync<object>(graphName, clxToAdd, new
            {
                Name = clxToAdd + "_vtx"
            });

            var response = await _client.GetVertexAsync<GetVertexMockModel>(graphName, clxToAdd, createVtxResponse.Vertex._key);

            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);
            Assert.False(response.ResponseDetails.Error);
            Assert.NotNull(response.Vertex);
            Assert.Equal(clxToAdd + "_vtx", response.Vertex.Name);
            Assert.Equal(createVtxResponse.Vertex._key, response.Vertex._key);
            Assert.Equal(createVtxResponse.Vertex._id, response.Vertex._id);
        }

        [Fact]
        public async Task GetVertexAsync_ShouldThrow_WhenVertexCollectionIsNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            // Create a new graph
            string graphName = nameof(GetVertexAsync_ShouldThrow_WhenVertexCollectionIsNotFound);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });
            string vertex = nameof(GetVertexAsync_ShouldThrow_WhenVertexCollectionIsNotFound) + "_vtx";

            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () => await _client.GetVertexAsync<GetVertexMockModel>(graphName, vertex, "12345"));

            Assert.True(ex.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1203, ex.ResponseDetails.ErrorNum); // ARANGO_DATA_SOURCE_NOT_FOUND
        }

        [Fact]
        public async Task GetVertexAsync_ShouldThrow_WhenVertexIsNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            // Create a new graph
            string graphName = nameof(GetVertexAsync_ShouldThrow_WhenVertexIsNotFound);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });
            string vertex = nameof(GetVertexAsync_ShouldThrow_WhenVertexIsNotFound) + "_vtx";

            await _client.PostVertexCollectionAsync(
                graphName,
                new PostVertexCollectionBody
                {
                    Collection = vertex
                });

            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () => await _client.GetVertexAsync<GetVertexMockModel>(graphName, vertex, "12456"));

            Assert.True(ex.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1202, ex.ResponseDetails.ErrorNum); // ARANGO_DOCUMENT_NOT_FOUND
        }

        [Fact]
        public async Task GetVertexAsync_ShouldThrow_WhenGraphIsNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            string graphName = nameof(GetVertexAsync_ShouldThrow_WhenGraphIsNotFound);
            string vertex = nameof(GetVertexAsync_ShouldThrow_WhenGraphIsNotFound) + "_vtx";

            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () => await _client.GetVertexAsync<GetVertexMockModel>(graphName, vertex, "12345"));

            Assert.True(ex.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1924, ex.ResponseDetails.ErrorNum); // ERROR_GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task DeleteVertexAsync_ShouldSucceed()
        {
            // Create a new graph

            string graphName = nameof(DeleteVertexAsync_ShouldSucceed);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });

            // Add a vertex collection

            string clxToAdd = nameof(DeleteVertexAsync_ShouldSucceed);

            await _client.PostVertexCollectionAsync(
                graphName,
                new PostVertexCollectionBody
                {
                    Collection = clxToAdd
                });

            var vertexProperty = clxToAdd + "_vtx";

            var createVtxResponse = await _client.PostVertexAsync(graphName, clxToAdd, new
            {
                Name = vertexProperty
            });

            Assert.Equal(HttpStatusCode.Accepted, createVtxResponse.ResponseDetails.Code);

            var response = await _client.DeleteVertexAsync<DeleteVertexMockModel>(graphName, clxToAdd, createVtxResponse.Vertex._key, new DeleteVertexQuery
            {
                ReturnOld = true,
                WaitForSync = true
            });

            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);
            Assert.False(response.ResponseDetails.Error);
            Assert.True(response.Removed);
            Assert.Equal(vertexProperty, response.Old.Name);
        }

        [Fact]
        public async Task DeleteVertexAsync_ShouldThrow_WhenGraphIsNotFound()
        {
            _client.ThrowErrorsAsExceptions = true;
            string graphName = nameof(DeleteVertexAsync_ShouldThrow_WhenGraphIsNotFound);
            string vertex = nameof(DeleteVertexAsync_ShouldThrow_WhenGraphIsNotFound) + "_vtx";

            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () => await _client.DeleteVertexAsync<object>(graphName, vertex, "12345"));

            Assert.True(ex.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1924, ex.ResponseDetails.ErrorNum); // ERROR_GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task DeleteVertexAsync_ShouldReturnError_WhenGraphIsNotFound()
        {
            string graphName = nameof(DeleteVertexAsync_ShouldThrow_WhenGraphIsNotFound);
            string vertex = nameof(DeleteVertexAsync_ShouldThrow_WhenGraphIsNotFound) + "_vtx";

            DeleteVertexResponse<object> deleteVertexResponse = await _client.DeleteVertexAsync<object>(graphName, vertex, "12345");
            
            Assert.False(deleteVertexResponse.IsSuccess);
            Assert.True(deleteVertexResponse.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, deleteVertexResponse.ResponseDetails.Code);
            Assert.Equal(1924, deleteVertexResponse.ResponseDetails.ErrorNum); // ERROR_GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task DeleteVertexAsync_ShouldThrow_WhenVertexCollectionIsNotFound()
        {
            _client.ThrowErrorsAsExceptions = true;
            string graphName = nameof(DeleteVertexAsync_ShouldThrow_WhenVertexCollectionIsNotFound);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });

            string vertex = nameof(DeleteVertexAsync_ShouldThrow_WhenVertexCollectionIsNotFound) + "_vtx";

            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () => await _client.DeleteVertexAsync<object>(graphName, vertex, "12345"));

            Assert.True(ex.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1203, ex.ResponseDetails.ErrorNum); // ARANGO_DATA_SOURCE_NOT_FOUND
        }

        [Fact]
        public async Task DeleteVertexAsync_ShouldReturnError_WhenVertexCollectionIsNotFound()
        {
            string graphName = nameof(DeleteVertexAsync_ShouldThrow_WhenVertexCollectionIsNotFound);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });

            string vertex = nameof(DeleteVertexAsync_ShouldThrow_WhenVertexCollectionIsNotFound) + "_vtx";

            DeleteVertexResponse<object> deleteVertexResponse = await _client.DeleteVertexAsync<object>(graphName, vertex, "12345");
            
            Assert.False(deleteVertexResponse.IsSuccess);
            Assert.True(deleteVertexResponse.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, deleteVertexResponse.ResponseDetails.Code);
            Assert.Equal(1203, deleteVertexResponse.ResponseDetails.ErrorNum); // ARANGO_DATA_SOURCE_NOT_FOUND
        }

        [Fact]
        public async Task DeleteVertexAsync_ShouldThrow_WhenVertexIsNotFound()
        {
            _client.ThrowErrorsAsExceptions = true;
            string graphName = nameof(DeleteVertexAsync_ShouldThrow_WhenVertexIsNotFound);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });

            string vertexClx = nameof(DeleteVertexAsync_ShouldThrow_WhenVertexIsNotFound) + "_vtxClx";

            await _client.PostVertexCollectionAsync(
                graphName,
                new PostVertexCollectionBody
                {
                    Collection = vertexClx
                });

            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
                await _client.DeleteVertexAsync<object>(graphName, vertexClx, "12345"));

            Assert.True(ex.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1202, ex.ResponseDetails.ErrorNum); // ARANGO_DOCUMENT_NOT_FOUND
        }

        [Fact]
        public async Task DeleteVertexAsync_ShouldReturnError_WhenVertexIsNotFound()
        {
            string graphName = nameof(DeleteVertexAsync_ShouldReturnError_WhenVertexIsNotFound);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });

            string vertexClx = nameof(DeleteVertexAsync_ShouldReturnError_WhenVertexIsNotFound) + "_vtxClx";

            await _client.PostVertexCollectionAsync(
                graphName,
                new PostVertexCollectionBody
                {
                    Collection = vertexClx
                });

            DeleteVertexResponse<object> deleteVertexResponse = await _client.DeleteVertexAsync<object>(graphName, vertexClx, "12345");
            
            Assert.False(deleteVertexResponse.IsSuccess);
            Assert.True(deleteVertexResponse.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, deleteVertexResponse.ResponseDetails.Code);
            Assert.Equal(1202, deleteVertexResponse.ResponseDetails.ErrorNum); // ARANGO_DOCUMENT_NOT_FOUND
        }

        [Fact]
        public async Task PatchVertexAsync_ShouldSucceed()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            // Create a new graph

            string graphName = nameof(PatchVertexAsync_ShouldSucceed);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });

            // Add a vertex collection

            string clxToAdd = nameof(PatchVertexAsync_ShouldSucceed);

            await _client.PostVertexCollectionAsync(
                graphName,
                new PostVertexCollectionBody
                {
                    Collection = clxToAdd
                });

            var createVtxResponse = await _client.PostVertexAsync(graphName, clxToAdd, new
            {
                Name = clxToAdd + "_vtx",
                Value = "myValue"
            }, new PostVertexOptions
            {
                ReturnNew = true,
                WaitForSync = true
            });

            var response = await _client.PatchVertexAsync<dynamic, PatchVertexMockModel>(graphName, clxToAdd, createVtxResponse.Vertex._key, new
            {
                Name = clxToAdd + "_vtx_2"
            }, new PatchVertexQuery
            {
                ReturnNew = true,
                ReturnOld = true,
                WaitForSync = true
            });

            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);
            Assert.False(response.ResponseDetails.Error);
            Assert.NotNull(response.Vertex);
            Assert.NotEqual(createVtxResponse.Vertex._rev, response.Vertex._rev);
            Assert.NotEqual(createVtxResponse.Vertex._rev, response.New._rev);
            Assert.NotEqual(createVtxResponse.New.Name, response.New.Name);
            Assert.Equal(createVtxResponse.New.Value, response.New.Value);
        }

        [Fact]
        public async Task PatchVertexAsync_ShouldThrow_WhenGraphIsNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            string graphName = nameof(PatchVertexAsync_ShouldThrow_WhenGraphIsNotFound);
            string vertex = nameof(PatchVertexAsync_ShouldThrow_WhenGraphIsNotFound) + "_vtx";

            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () => await _client.PatchVertexAsync<dynamic, PatchVertexMockModel>(graphName, vertex, "12345", new { }));

            Assert.True(ex.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1924, ex.ResponseDetails.ErrorNum); // ERROR_GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task PatchVertexAsync_ShouldThrow_WhenVertexCollectionIsNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            // Create a new graph
            string graphName = nameof(PatchVertexAsync_ShouldThrow_WhenVertexCollectionIsNotFound);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });
            string vertex = nameof(PatchVertexAsync_ShouldThrow_WhenVertexCollectionIsNotFound) + "_vtx";

            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () => await _client.PatchVertexAsync<dynamic, PatchVertexMockModel>(graphName, vertex, "12345", new { }));

            Assert.True(ex.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1203, ex.ResponseDetails.ErrorNum); // ARANGO_DATA_SOURCE_NOT_FOUND
        }

        [Fact]
        public async Task PatchVertexAsync_ShouldThrow_WhenVertexIsNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            // Create a new graph
            string graphName = nameof(PatchVertexAsync_ShouldThrow_WhenVertexIsNotFound);

            await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });
            string vertexClx = nameof(PatchVertexAsync_ShouldThrow_WhenVertexIsNotFound) + "_vtxClx";

            await _client.PostVertexCollectionAsync(
                graphName,
                new PostVertexCollectionBody
                {
                    Collection = vertexClx
                });

            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () => await _client.PatchVertexAsync<dynamic, PatchVertexMockModel>(graphName, vertexClx, "12345", new { }));

            Assert.True(ex.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1202, ex.ResponseDetails.ErrorNum); // ARANGO_DOCUMENT_NOT_FOUND
        }

        [Fact]
        public async Task PutGraphEdgeAsync_ShouldSucceed()
        {
            string graphName = nameof(PutGraphEdgeAsync_ShouldSucceed);
            string fromClx = graphName + "_fromclx";
            string toClx = graphName + "_toclx";
            string edgeClx = graphName + "_edgeclx";

            // Create a new graph

            await _fixture.ArangoDBClient.Graph.PostGraphAsync(new PostGraphBody
            {
                Name = graphName,
                EdgeDefinitions = new List<EdgeDefinition>
                {
                    new EdgeDefinition
                    {
                        From = new[] { fromClx },
                        To = new[] { toClx },
                        Collection = edgeClx
                    }
                }
            });

            // Create a document in the vertex collections

            PostDocumentResponse<object> fromResponse = await
                _fixture.ArangoDBClient.Document.PostDocumentAsync<object>(
                fromClx,
                new { myKey = "myValue" });

            PostDocumentResponse<object> toResponse = await
                _fixture.ArangoDBClient.Document.PostDocumentAsync<object>(
                toClx,
                new { myKey = "myValue" });

            // Create the edge

            var createEdgeResponse = await _client.PostEdgeAsync(
                graphName,
                edgeClx,
                new
                {
                    _from = fromResponse._id,
                    _to = toResponse._id,
                    myKey = "myValue"
                },
                new PostEdgeQuery
                {
                    ReturnNew = true,
                    WaitForSync = true
                });

            var response = await _client.PutEdgeAsync(graphName, edgeClx, createEdgeResponse.Edge._key, new
            {
                _from = fromResponse._id,
                _to = toResponse._id,
                myKey = "newValue"
            }, new PutEdgeQuery
            {
                ReturnNew = true,
                ReturnOld = true,
                WaitForSync = true
            });

            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);
            Assert.Equal(createEdgeResponse.New.myKey, response.Old.myKey);
            Assert.NotEqual(createEdgeResponse.New.myKey, response.New.myKey);
            Assert.False(response.ResponseDetails.Error);
            Assert.NotEqual(response.Edge._rev, createEdgeResponse.Edge._rev);
        }

        [Fact]
        public async Task PuGraphEdgeAsync_ShouldThrow_WhenGraphNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            string graphName = nameof(PuGraphEdgeAsync_ShouldThrow_WhenGraphNotFound);

            var exception = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _client.PutEdgeAsync(graphName, "edgeClx", "", new
                {
                    myKey = "myValue"
                });
            });

            Assert.Equal(HttpStatusCode.NotFound, exception.ResponseDetails.Code);
            Assert.Equal(1924, exception.ResponseDetails.ErrorNum); // GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task PutEdgeDefinitionAsync_ShouldSucceed()
        {
            var graphClient = _fixture.PutEdgeDefinitionAsync_ShouldSucceed_ArangoDBClient.Graph;
            string edgeClx = nameof(PutEdgeDefinitionAsync_ShouldSucceed);
            var response = await graphClient.PutEdgeDefinitionAsync(
                _fixture.TestGraph,
                edgeClx,
                new PutEdgeDefinitionBody
                {
                    Collection = edgeClx,
                    // (update is to swap the direction of from and to)
                    To = new[] { "fromclx" },
                    From = new[] { "toclx" }
                });

            Assert.Equal(HttpStatusCode.Accepted, response.ResponseDetails.Code);
            Assert.False(response.ResponseDetails.Error);

            var newEdgeDef = response.Graph.EdgeDefinitions.FirstOrDefault();
            string afterFromDefinition = newEdgeDef.From.FirstOrDefault();
            string afterToDefinition = newEdgeDef.To.FirstOrDefault();
            Assert.NotEqual("fromclx", afterFromDefinition);
            Assert.NotEqual("toclx", afterToDefinition);
        }

        [Fact]
        public async Task PutEdgeDefinitionAsync_ShouldThrow_WhenGraphNameDoesNotExist()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            var edgeClx = nameof(PutEdgeDefinitionAsync_ShouldThrow_WhenGraphNameDoesNotExist) + "_edgeClx";
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _client.PutEdgeDefinitionAsync("bogus_collection", edgeClx, new PutEdgeDefinitionBody
                {
                    Collection = edgeClx,
                    To = new[] { "ToClx" },
                    From = new[] { "FromClx" }
                }, new PutEdgeDefinitionQuery
                {
                    WaitForSync = false
                });
            });

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1924, ex.ResponseDetails.ErrorNum); // GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task PutEdgeDefinitionAsync_ShouldThrow_WhenEdgeCollectionNameDoesNotExist()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _client.PutEdgeDefinitionAsync(_fixture.TestGraph, "bogus_edgeclx", new PutEdgeDefinitionBody
                {
                    Collection = "bogus_edgeclx",
                    To = new[] { "ToClx" },
                    From = new[] { "FromClx" }
                }, new PutEdgeDefinitionQuery
                {
                    WaitForSync = false
                });
            });

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1930, ex.ResponseDetails.ErrorNum); // GRAPH_EDGE_COLLECTION_NOT_USED
        }

        [Fact]
        public async Task GetEdgeAsync_ShouldSucceed()
        {
            string graphName = nameof(GetEdgeAsync_ShouldSucceed);
            string fromClx = graphName + "_fromclx";
            string toClx = graphName + "_toclx";
            string edgeClx = graphName + "_edgeclx";

            // Create a new graph

            await _fixture.ArangoDBClient.Graph.PostGraphAsync(new PostGraphBody
            {
                Name = graphName,
                EdgeDefinitions = new List<EdgeDefinition>
                {
                    new EdgeDefinition
                    {
                        From = new[] { fromClx },
                        To = new[] { toClx },
                        Collection = edgeClx
                    }
                }
            });

            // Create a document in the vertex collections

            PostDocumentResponse<object> fromResponse = await
                _fixture.ArangoDBClient.Document.PostDocumentAsync<object>(
                fromClx,
                new { myKey = "myValue" });

            PostDocumentResponse<object> toResponse = await
                _fixture.ArangoDBClient.Document.PostDocumentAsync<object>(
                toClx,
                new { myKey = "myValue" });

            // Create the edge

            var createdEdgeResponse = await _client.PostEdgeAsync(
                graphName,
                edgeClx,
                new
                {
                    _from = fromResponse._id,
                    _to = toResponse._id,
                    myKey = "myValue"
                },
                new PostEdgeQuery
                {
                    ReturnNew = true,
                    WaitForSync = true
                });

            // Get the edge with collection name and _key

            var response = await _client.GetEdgeAsync<Newtonsoft.Json.Linq.JObject>(
                graphName,
                edgeClx,
                createdEdgeResponse.Edge._key);

            Assert.NotNull(response.Edge);
            Assert.Equal("myValue", response.Edge["myKey"].ToString());

            // Get the edge with document-handle

            response = await _client.GetEdgeAsync<Newtonsoft.Json.Linq.JObject>(
                graphName,
                createdEdgeResponse.Edge._id);

            Assert.NotNull(response.Edge);
            Assert.Equal("myValue", response.Edge["myKey"].ToString());
        }

        [Fact]
        public async Task GetEdgeAsync_ShouldThrow_WhenEdgeWithRevisionIsNotFound()
        {
            _client.ThrowErrorsAsExceptions = true;
            string graphName = nameof(GetEdgeAsync_ShouldThrow_WhenEdgeWithRevisionIsNotFound);
            string fromClx = graphName + "_fromclx";
            string toClx = graphName + "_toclx";
            string edgeClx = graphName + "_edgeclx";

            // Create a new graph

            await _fixture.ArangoDBClient.Graph.PostGraphAsync(new PostGraphBody
            {
                Name = graphName,
                EdgeDefinitions = new List<EdgeDefinition>
                {
                    new EdgeDefinition
                    {
                        From = new[] { fromClx },
                        To = new[] { toClx },
                        Collection = edgeClx
                    }
                }
            });

            // Create a document in the vertex collections

            PostDocumentResponse<object> fromResponse = await
                _fixture.ArangoDBClient.Document.PostDocumentAsync<object>(
                fromClx,
                new { myKey = "myValue" });

            PostDocumentResponse<object> toResponse = await
                _fixture.ArangoDBClient.Document.PostDocumentAsync<object>(
                toClx,
                new { myKey = "myValue" });

            // Create the edge

            var createdEdgeResponse = await _client.PostEdgeAsync(
                graphName,
                edgeClx,
                new
                {
                    _from = fromResponse._id,
                    _to = toResponse._id,
                    myKey = "myValue"
                },
                new PostEdgeQuery
                {
                    ReturnNew = true,
                    WaitForSync = true
                });

            // Get the edge with a non-existing revision

            var exception = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _client.GetEdgeAsync<Newtonsoft.Json.Linq.JObject>(
                graphName,
                edgeClx,
                createdEdgeResponse.Edge._key,
                new GetEdgeQuery
                {
                    Rev = "RevisionThatDoesNotExist"
                });
            });

            Assert.Equal(HttpStatusCode.PreconditionFailed, exception.ResponseDetails.Code);
            Assert.Equal(1200, exception.ResponseDetails.ErrorNum); // ERROR_ARANGO_CONFLICT
        }

        [Fact]
        public async Task GetEdgeAsync_ShouldReturnError_WhenEdgeWithRevisionIsNotFound()
        {
            string graphName = nameof(GetEdgeAsync_ShouldThrow_WhenEdgeWithRevisionIsNotFound);
            string fromClx = graphName + "_fromclx";
            string toClx = graphName + "_toclx";
            string edgeClx = graphName + "_edgeclx";

            // Create a new graph

            await _fixture.ArangoDBClient.Graph.PostGraphAsync(new PostGraphBody
            {
                Name = graphName,
                EdgeDefinitions = new List<EdgeDefinition>
                {
                    new EdgeDefinition
                    {
                        From = new[] { fromClx },
                        To = new[] { toClx },
                        Collection = edgeClx
                    }
                }
            });

            // Create a document in the vertex collections

            PostDocumentResponse<object> fromResponse = await
                _fixture.ArangoDBClient.Document.PostDocumentAsync<object>(
                fromClx,
                new { myKey = "myValue" });

            PostDocumentResponse<object> toResponse = await
                _fixture.ArangoDBClient.Document.PostDocumentAsync<object>(
                toClx,
                new { myKey = "myValue" });

            // Create the edge

            var createdEdgeResponse = await _client.PostEdgeAsync(
                graphName,
                edgeClx,
                new
                {
                    _from = fromResponse._id,
                    _to = toResponse._id,
                    myKey = "myValue"
                },
                new PostEdgeQuery
                {
                    ReturnNew = true,
                    WaitForSync = true
                });

            // Get the edge with a non-existing revision

            GetEdgeResponse<JObject> getEdgeResponse = await _client.GetEdgeAsync<Newtonsoft.Json.Linq.JObject>(
                graphName,
                edgeClx,
                createdEdgeResponse.Edge._key,
                new GetEdgeQuery
                {
                    Rev = "RevisionThatDoesNotExist"
                });

            Assert.False(getEdgeResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.PreconditionFailed, getEdgeResponse.ResponseDetails.Code);
            Assert.Equal(1200, getEdgeResponse.ResponseDetails.ErrorNum); // ERROR_ARANGO_CONFLICT
        }

        [Fact]
        public async Task GetEdgeAsync_ShouldThrow_WhenGraphIsNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            var exception = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _client.GetEdgeAsync<Newtonsoft.Json.Linq.JObject>(
                    nameof(GetEdgeAsync_ShouldThrow_WhenGraphIsNotFound),
                    "edgeClx",
                    "0123456789");
            });

            Assert.Equal(HttpStatusCode.NotFound, exception.ResponseDetails.Code);
            Assert.Equal(1924, exception.ResponseDetails.ErrorNum); // GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task PatchEdgeAsync_ShouldSucceed()
        {
            string graphName = nameof(PatchEdgeAsync_ShouldSucceed);
            string fromClx = graphName + "_fromclx";
            string toClx = graphName + "_toclx";
            string edgeClx = graphName + "_edgeclx";

            // Create a new graph

            await _fixture.ArangoDBClient.Graph.PostGraphAsync(new PostGraphBody
            {
                Name = graphName,
                EdgeDefinitions = new List<EdgeDefinition>
                {
                    new EdgeDefinition
                    {
                        From = new[] { fromClx },
                        To = new[] { toClx },
                        Collection = edgeClx
                    }
                }
            });

            // Create a document in the vertex collections

            PostDocumentResponse<object> fromResponse = await
                _fixture.ArangoDBClient.Document.PostDocumentAsync<object>(
                fromClx,
                new { myKey = "myValue" });

            PostDocumentResponse<object> toResponse = await
                _fixture.ArangoDBClient.Document.PostDocumentAsync<object>(
                toClx,
                new { myKey = "myValue" });

            // Create the edge

            var createEdgeResponse = await _client.PostEdgeAsync(
                graphName,
                edgeClx,
                new
                {
                    _from = fromResponse._id,
                    _to = toResponse._id,
                    myKey = "myValue",
                    value = 1
                },
                new PostEdgeQuery
                {
                    ReturnNew = true,
                    WaitForSync = true
                });

            var response = await _client.PatchEdgeAsync<object, PatchEdgeMockModel>(
                graphName,
                edgeClx,
                createEdgeResponse.Edge._key,
                new
                {
                    _from = fromResponse._id,
                    _to = toResponse._id,
                    myKey = "newValue"
                }, new PatchEdgeQuery
                {
                    ReturnNew = true,
                    ReturnOld = true,
                    WaitForSync = true
                });

            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);
            Assert.Equal(createEdgeResponse.New.myKey, response.Old.myKey);
            Assert.NotEqual(createEdgeResponse.New.myKey, response.New.myKey);
            Assert.False(response.ResponseDetails.Error);
            Assert.NotEqual(createEdgeResponse.Edge._rev, response.Edge._rev);
            Assert.Equal(createEdgeResponse.New.value, response.New.value);
            Assert.Equal(createEdgeResponse.New.value, response.Old.value);
        }

        [Fact]
        public async Task PatchEdgeAsync_ShouldThrow_WhenGraphNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            string graphName = nameof(PatchEdgeAsync_ShouldThrow_WhenGraphNotFound);

            var exception = await Assert.ThrowsAsync<ApiErrorException>(async () =>
                await _client.PatchEdgeAsync<object, PatchEdgeMockModel>(graphName, "edgeClx", "", new { }));

            Assert.Equal(HttpStatusCode.NotFound, exception.ResponseDetails.Code);
            Assert.Equal(1924, exception.ResponseDetails.ErrorNum); // GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task PutVertexAsync_ShouldSuceed()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            string graphName = nameof(PutVertexAsync_ShouldSuceed) + "_graph";

            PostGraphResponse createGraphResponse = await _client.PostGraphAsync(
                new PostGraphBody
                {
                    Name = graphName
                });

            Assert.Equal(HttpStatusCode.Accepted, createGraphResponse.ResponseDetails.Code);

            string vertexClx = nameof(PutVertexAsync_ShouldSuceed) + "_clx";

            await _client.PostVertexCollectionAsync(
                graphName,
                new PostVertexCollectionBody
                {
                    Collection = vertexClx
                });

            var beforeVertexClxVtx = vertexClx + "_vtx";
            var afterVertexClxVtx = vertexClx + "_vtx_2";

            var createVertexResponse = await _client.PostVertexAsync(graphName, vertexClx, new
            {
                Name = beforeVertexClxVtx
            });
            var response = await _client.PutVertexAsync(graphName, vertexClx, createVertexResponse.Vertex._key, new PutVertexMockModel
            {
                Name = afterVertexClxVtx
            }, new PutVertexQuery
            {
                ReturnNew = true,
                ReturnOld = true,
                WaitForSync = true
            });

            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);
            Assert.False(response.ResponseDetails.Error);
            Assert.Equal(afterVertexClxVtx, response.New.Name);
            Assert.NotEqual(response.New.Name, response.Old.Name);
            Assert.NotEqual(createVertexResponse.Vertex._rev, response.Vertex._rev);
        }

        [Fact]
        public async Task PutVertexAsync_ShouldThrow_WhenGraphIsNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            string vertexClx = nameof(PutVertexAsync_ShouldThrow_WhenGraphIsNotFound);
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _client.PutVertexAsync("bogusGraph", vertexClx, "", new PutVertexMockModel
                {
                    Name = "Bogus_Name"
                });
            });

            Assert.True(ex.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1924, ex.ResponseDetails.ErrorNum); // ERROR_GRAPH_NOT_FOUND
        }

        [Fact]
        public async Task PutVertexAsync_ShouldThrow_WhenVertexCollectionIsNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            string graphName = nameof(PutVertexAsync_ShouldThrow_WhenVertexCollectionIsNotFound);
            string vertexClx = nameof(PutVertexAsync_ShouldThrow_WhenVertexCollectionIsNotFound);

            await _client.PostGraphAsync(new PostGraphBody { Name = graphName });

            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _client.PutVertexAsync(graphName, vertexClx, "12345", new PutVertexMockModel
                {
                    Name = "Bogus_Name"
                });
            });
            Assert.True(ex.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1203, ex.ResponseDetails.ErrorNum); // ARANGO_DATA_SOURCE_NOT_FOUND
        }

        [Fact]
        public async Task PutVertexAsync_ShouldThrow_WhenVertexIsNotFound()
        {
            //TODO Error handling test
            _client.ThrowErrorsAsExceptions = true;
            string graphName = nameof(PutVertexAsync_ShouldThrow_WhenGraphIsNotFound);
            string vertexClx = nameof(PutVertexAsync_ShouldThrow_WhenGraphIsNotFound);

            await _client.PostGraphAsync(new PostGraphBody { Name = graphName });
            await _client.PostVertexCollectionAsync(graphName, new PostVertexCollectionBody
            {
                Collection = vertexClx
            });


            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _client.PutVertexAsync(graphName, vertexClx, "123456", new PutVertexMockModel
                {
                    Name = "Bogus_Name"
                });
            });

            Assert.True(ex.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
            Assert.Equal(1202, ex.ResponseDetails.ErrorNum); // ARANGO_DOCUMENT_NOT_FOUND
        }
    }
}
