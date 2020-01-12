using ArangoDBNetStandard;
using ArangoDBNetStandard.CollectionApi;
using ArangoDBNetStandard.CollectionApi.Models;
using ArangoDBNetStandard.DocumentApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ArangoDBNetStandardTest.CollectionApi
{
    public class CollectionApiClientTest : IClassFixture<CollectionApiClientTestFixture>
    {
        private readonly CollectionApiClient _collectionApi;
        private readonly ArangoDBClient _adb;
        private readonly string _testCollection;

        public CollectionApiClientTest(CollectionApiClientTestFixture fixture)
        {
            _adb = fixture.ArangoDBClient;
            _collectionApi = _adb.Collection;
            _collectionApi.ThrowErrorsAsExceptions = false;
            _testCollection = fixture.TestCollection;

            // Truncate TestCollection before each test
            _collectionApi.TruncateCollectionAsync(fixture.TestCollection)
                .GetAwaiter()
                .GetResult();
        }

        [Fact]
        public async Task DeleteCollectionAsync_ShouldSucceed()
        {
            string clx = "DeleteCollectionAsync_ShouldSucceed";

            // create a collection so we can delete it
            var createResponse = await _collectionApi.PostCollectionAsync(
                new PostCollectionBody
                {
                    Name = clx
                });
            string clxId = createResponse.Id;
            Assert.False(createResponse.ResponseDetails.Error);
            Assert.NotNull(clxId);

            var deleteResponse = await _collectionApi.DeleteCollectionAsync(clx);
            Assert.False(deleteResponse.ResponseDetails.Error);
            Assert.Equal(clxId, deleteResponse.Id);
        }

        [Fact]
        public async Task DeleteCollectionAsync_ShouldThrow_WhenCollectionDoesNotExist()
        {
            _collectionApi.ThrowErrorsAsExceptions = true;

            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
                await _collectionApi.DeleteCollectionAsync("NotACollection"));
            Assert.Equal(1203, ex.ResponseDetails.ErrorNum);
        }

        [Fact]
        public async Task DeleteCollectionAsync_ShouldReturnError_WhenCollectionDoesNotExist()
        {
            DeleteCollectionResponse deleteCollectionResponse = await _collectionApi.DeleteCollectionAsync("NotACollection");

            Assert.False(deleteCollectionResponse.IsSuccess);
            Assert.True(deleteCollectionResponse.ResponseDetails.Error);
            Assert.Equal(1203, deleteCollectionResponse.ResponseDetails.ErrorNum);
        }

        [Fact]
        public async Task PostCollectionAsync_ShouldSucceed()
        {
            var response = await _collectionApi.PostCollectionAsync(
                new PostCollectionBody
                {
                    Name = "MyCollection"
                });

            Assert.False(response.ResponseDetails.Error);
            Assert.NotNull(response.Id);
            Assert.Equal("MyCollection", response.Name);
            Assert.Equal("traditional", response.KeyOptions.Type);
            Assert.Equal(2, response.Type); // 2 is document collection, 3 is edge collection
        }

        [Fact]
        public async Task PostCollectionAsync_ShouldSucceed_WhenUsingKeyOptions()
        {
            var response = await _collectionApi.PostCollectionAsync(
                new PostCollectionBody
                {
                    Name = "MyCollectionWithKeyOptions",
                    KeyOptions = new CollectionKeyOptions(false, 5, 1,"autoincrement")
                });

            Assert.False(response.ResponseDetails.Error);
            Assert.NotNull(response.Id);
            Assert.Equal("MyCollectionWithKeyOptions", response.Name);
            Assert.Equal("autoincrement", response.KeyOptions.Type);
            Assert.False(response.KeyOptions.AllowUserKeys);
            Assert.Equal(2, response.Type); // 2 is document collection, 3 is edge collection
        }


        [Fact]
        public async Task PostCollectionAsync_ShouldSucceed_WhenEdgeCollection()
        {
            var response = await _collectionApi.PostCollectionAsync(
                new PostCollectionBody
                {
                    Name = "MyEdgeCollection",
                    Type = 3
                });

            Assert.False(response.ResponseDetails.Error);
            Assert.NotNull(response.Id);
            Assert.Equal("MyEdgeCollection", response.Name);
            Assert.Equal("traditional", response.KeyOptions.Type);
            Assert.Equal(3, response.Type); // 2 is document collection, 3 is edge collection
        }

        [Fact]
        public async Task PostCollectionAsync_ShouldThrow_WhenCollectionNameExists()
        {
            _collectionApi.ThrowErrorsAsExceptions = true;
            var request = new PostCollectionBody
            {
                Name = "MyOneAndOnlyCollection"
            };
            await _collectionApi.PostCollectionAsync(request);
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () => await _collectionApi.PostCollectionAsync(request));
            Assert.Equal(1207, ex.ResponseDetails.ErrorNum);
        }

        [Fact]
        public async Task PostCollectionAsync_ShouldReturnError_WhenCollectionNameExists()
        {
            var request = new PostCollectionBody
            {
                Name = "MyOneAndOnlyCollection"
            };
            await _collectionApi.PostCollectionAsync(request);
            PostCollectionResponse postCollectionResponse = await _collectionApi.PostCollectionAsync(request);

            Assert.False(postCollectionResponse.IsSuccess);
            Assert.True(postCollectionResponse.ResponseDetails.Error);
            Assert.Equal(1207, postCollectionResponse.ResponseDetails.ErrorNum);
        }

        [Fact]
        public async Task PostCollectionAsync_ShouldThrow_WhenCollectionNameInvalid()
        {
            _collectionApi.ThrowErrorsAsExceptions = true;
            var request = new PostCollectionBody
            {
                Name = "My collection name with spaces"
            };
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () => await _collectionApi.PostCollectionAsync(request));
            Assert.Equal(1208, ex.ResponseDetails.ErrorNum);
        }

        [Fact]
        public async Task PostCollectionAsync_ShouldReturnError_WhenCollectionNameInvalid()
        {
            var request = new PostCollectionBody
            {
                Name = "My collection name with spaces"
            };
            PostCollectionResponse postCollectionResponse = await _collectionApi.PostCollectionAsync(request);

            Assert.False(postCollectionResponse.IsSuccess);
            Assert.True(postCollectionResponse.ResponseDetails.Error);
            Assert.Equal(1208, postCollectionResponse.ResponseDetails.ErrorNum);
        }

        [Fact]
        public async Task TruncateCollectionAsync_ShouldSucceed()
        {
            // add a document
            var response = await _adb.Document.PostDocumentAsync<object>(
                _testCollection,
                new { test = 123 });
            Assert.NotNull(response._id);

            // truncate collection
            var result = await _collectionApi.TruncateCollectionAsync(_testCollection);

            // count documents in collection, should be zero
            int count = (await _adb.Cursor.PostCursorAsync<int>(
                query: "RETURN COUNT(@@clx)",
                bindVars: new Dictionary<string, object> { ["@clx"] = _testCollection }))
                .Results
                .First();

            Assert.Equal(0, count);

            Assert.Equal(HttpStatusCode.OK, result.ResponseDetails.Code);
            Assert.False(result.ResponseDetails.Error);
            Assert.NotNull(result.Id);
            Assert.NotNull(result.GloballyUniqueId);
            Assert.Equal(2, result.Type);
            Assert.Equal(3, result.Status);
            Assert.False(result.IsSystem);
            Assert.Equal(_testCollection, result.Name);
        }

        [Fact]
        public async Task TruncateCollectionAsync_ShouldThrow_WhenCollectionDoesNotExist()
        {
            _collectionApi.ThrowErrorsAsExceptions = true;
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
                await _collectionApi.TruncateCollectionAsync("NotACollection"));

            Assert.Equal(1203, ex.ResponseDetails.ErrorNum);
        }

        [Fact]
        public async Task TruncateCollectionAsync_ShouldReturnError_WhenCollectionDoesNotExist()
        {
            TruncateCollectionResponse truncateCollectionResponse = await _collectionApi.TruncateCollectionAsync("NotACollection");
            
            Assert.False(truncateCollectionResponse.IsSuccess);
            Assert.True(truncateCollectionResponse.ResponseDetails.Error);
            Assert.Equal(1203, truncateCollectionResponse.ResponseDetails.ErrorNum);
        }

        [Fact]
        public async Task GetCollectionCountAsync_ShouldSucceed()
        {
            var newDoc = await _adb.Document.PostDocumentAsync(_testCollection, new PostDocumentsOptions());
            var response = await _collectionApi.GetCollectionCountAsync(_testCollection);

            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);
            Assert.False(response.ResponseDetails.Error);
            Assert.False(response.IsSystem);
            Assert.Equal(3, response.Status);
            Assert.Equal("loaded", response.StatusString);
            Assert.Equal(2, response.Type);
            Assert.False(response.WaitForSync);
            Assert.NotNull(response.GloballyUniqueId);
            Assert.NotNull(response.Id);
            Assert.NotNull(response.KeyOptions);
            Assert.False(response.WaitForSync);
            Assert.Equal(1, response.Count);
            await _adb.Document.DeleteDocumentAsync(newDoc._id);
        }

        [Fact]
        public async Task GetCollectionCountAsync_ShouldThrow_WhenCollectionDoesNotExist()
        {
            _collectionApi.ThrowErrorsAsExceptions = true;
            var exception = await Assert.ThrowsAsync<ApiErrorException>(async () =>
                await _collectionApi.GetCollectionCountAsync("bogusCollection"));
            Assert.Equal(HttpStatusCode.NotFound, exception.ResponseDetails.Code);
        }

        [Fact]
        public async Task GetCollectionCountAsync_ShouldReturnError_WhenCollectionDoesNotExist()
        {
            GetCollectionCountResponse getCollectionCountResponse = await _collectionApi.GetCollectionCountAsync("bogusCollection");
            
            Assert.False(getCollectionCountResponse.IsSuccess);
            Assert.True(getCollectionCountResponse.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, getCollectionCountResponse.ResponseDetails.Code);
        }

        [Fact]
        public async Task GetCollectionsAsync_ShouldSucceed()
        {
            var response = await _collectionApi.GetCollectionsAsync(new GetCollectionsOptions
            {
                ExcludeSystem = true // System adds 9 collections that we don't need to test
            });
            Assert.NotEmpty(response);
            var collectionExists = response.Where(x => x.Name == _testCollection);

            Assert.False(response.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);
            Assert.NotNull(collectionExists);
        }

        [Fact]
        public async Task GetCollectionAsync_ShouldSucceed()
        {
            var collection = await _collectionApi.GetCollectionAsync(_testCollection);

            Assert.Equal(_testCollection, collection.Name);
        }

        [Fact]
        public async Task GetCollectionAsync_ShouldThrow_WhenNotFound()
        {
            _collectionApi.ThrowErrorsAsExceptions = true;
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () => await _collectionApi.GetCollectionAsync("MyWrongCollection"));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseDetails.Code);
        }

        [Fact]
        public async Task GetCollectionAsync_ShouldReturnError_WhenNotFound()
        {
            GetCollectionResponse getCollectionResponse = await _collectionApi.GetCollectionAsync("MyWrongCollection");

            Assert.False(getCollectionResponse.IsSuccess);
            Assert.True(getCollectionResponse.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, getCollectionResponse.ResponseDetails.Code);
        }

        [Fact]
        public async Task GetCollectionPropertiesAsync_ShouldSucceed()
        {
            var response = await _collectionApi.GetCollectionPropertiesAsync(_testCollection);

            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);
            Assert.NotNull(response.KeyOptions);
            Assert.False(response.WaitForSync);
            Assert.Equal(_testCollection, response.Name);
            // ObjectId is null, using ArangoDB 3.4.6 Enterprise Edition for Windows
            // Assert.NotNull(response.ObjectId);
            Assert.False(response.IsSystem);
            Assert.Equal(3, response.Status);
            Assert.Equal(2, response.Type);
        }

        [Fact]
        public async Task RenameCollectionAsync_ShouldSucceed()
        {
            string initialClx = nameof(RenameCollectionAsync_ShouldSucceed);
            string renamedClx = initialClx + "_Renamed";

            await _adb.Collection.PostCollectionAsync(
                    new PostCollectionBody
                    {
                        Name = initialClx
                    });
            var response = await _collectionApi.RenameCollectionAsync(initialClx, new RenameCollectionBody
            {
                Name = renamedClx
            });
            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);
            Assert.Equal(renamedClx, response.Name);
            Assert.False(response.IsSystem);
            Assert.NotNull(response.Id);
            Assert.False(response.ResponseDetails.Error);
        }

        [Fact]
        public async Task RenameCollectionAsync_ShouldThrow_WhenCollectionNotFound()
        {
            _collectionApi.ThrowErrorsAsExceptions = true;
            var exception = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _collectionApi.RenameCollectionAsync("bogusCollection", new RenameCollectionBody
                {
                    Name = "testingCollection"
                });
            });
            Assert.Equal(HttpStatusCode.NotFound, exception.ResponseDetails.Code);
            Assert.Equal(1203, exception.ResponseDetails.ErrorNum); // ARANGO_DATA_SOURCE_NOT_FOUND
        }

        [Fact]
        public async Task RenameCollectionAsync_ShouldReturnError_WhenCollectionNotFound()
        {
            RenameCollectionResponse renameCollectionResponse = await _collectionApi.RenameCollectionAsync("bogusCollection", new RenameCollectionBody
            {
                Name = "testingCollection"
            });

            Assert.False(renameCollectionResponse.IsSuccess);
            Assert.True(renameCollectionResponse.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, renameCollectionResponse.ResponseDetails.Code);
            Assert.Equal(1203, renameCollectionResponse.ResponseDetails.ErrorNum); // ARANGO_DATA_SOURCE_NOT_FOUND
        }

        [Fact]
        public async Task RenameCollectionAsync_ShouldThrow_WhenNameInvalid()
        {
            _collectionApi.ThrowErrorsAsExceptions = true;
            var exception = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _collectionApi.RenameCollectionAsync(_testCollection, new RenameCollectionBody
                {
                    Name = "Bad Collection Name"
                });
            });
            Assert.Equal(1208, exception.ResponseDetails.ErrorNum); // Arango Illegal Name
        }

        [Fact]
        public async Task RenameCollectionAsync_ShouldReturnError_WhenNameInvalid()
        {
            RenameCollectionResponse renameCollectionResponse = await _collectionApi.RenameCollectionAsync(_testCollection, new RenameCollectionBody
            {
                Name = "Bad Collection Name"
            });

            Assert.False(renameCollectionResponse.IsSuccess);
            Assert.True(renameCollectionResponse.ResponseDetails.Error);
            Assert.Equal(1208, renameCollectionResponse.ResponseDetails.ErrorNum); // Arango Illegal Name
        }

        [Fact]
        public async Task RenameCollectionAsync_ShouldThrow_WhenCollectionInvalid()
        {
            _collectionApi.ThrowErrorsAsExceptions = true;
            var exception = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _collectionApi.RenameCollectionAsync("Bad Collection Name", new RenameCollectionBody
                {
                    Name = "testingCollection"
                });
            });
            Assert.Equal(1203, exception.ResponseDetails.ErrorNum); // Arango Data Source Not Found
        }

        [Fact]
        public async Task RenameCollectionAsync_ShouldReturnError_WhenCollectionInvalid()
        {
            RenameCollectionResponse renameCollectionResponse = await _collectionApi.RenameCollectionAsync("Bad Collection Name", new RenameCollectionBody
            {
                Name = "testingCollection"
            });

            Assert.False(renameCollectionResponse.IsSuccess);
            Assert.True(renameCollectionResponse.ResponseDetails.Error);
            Assert.Equal(1203, renameCollectionResponse.ResponseDetails.ErrorNum); // Arango Data Source Not Found
        }

        [Fact]
        public async Task GetCollectionRevisionAsync_ShouldSucceed()
        {
            var response = await _collectionApi.GetCollectionRevisionAsync(_testCollection);
            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);
            Assert.Equal(_testCollection, response.Name);
            Assert.NotNull(response.Id);
            Assert.NotNull(response.KeyOptions);
            Assert.NotNull(response.Revision);
            Assert.NotNull(response.StatusString);
        }

        [Fact]
        public async Task GetCollectionRevisionAsync_ShouldThrow_WhenCollectionNotFound()
        {
            _collectionApi.ThrowErrorsAsExceptions = true;
            var exception = await Assert.ThrowsAsync<ApiErrorException>(async () =>
                await _collectionApi.GetCollectionRevisionAsync("bogusCollection"));
            Assert.Equal(HttpStatusCode.NotFound, exception.ResponseDetails.Code);
        }

        [Fact]
        public async Task GetCollectionRevisionAsync_ShouldReturnError_WhenCollectionNotFound()
        {
            GetCollectionRevisionResponse getCollectionRevisionResponse = await _collectionApi.GetCollectionRevisionAsync("bogusCollection");

            Assert.False(getCollectionRevisionResponse.IsSuccess);
            Assert.True(getCollectionRevisionResponse.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, getCollectionRevisionResponse.ResponseDetails.Code);
        }

        [Fact]
        public async Task PutCollectionPropertyAsync_ShouldSucceed()
        {
            var putCollection = await _adb.Collection.PostCollectionAsync(new PostCollectionBody
            {
                 Name = nameof(PutCollectionPropertyAsync_ShouldSucceed)
            });
            var beforeResponse = await _collectionApi.GetCollectionPropertiesAsync(putCollection.Name);

            var body = new PutCollectionPropertyBody
            {
                WaitForSync = !beforeResponse.WaitForSync
            };
            var response = await _collectionApi.PutCollectionPropertyAsync(putCollection.Name, body);

            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);
            Assert.NotEqual(beforeResponse.WaitForSync, response.WaitForSync);
        }

        [Fact]
        public async Task PutCollectionPropertyAsync_ShouldThrow_WhenCollectionDoesNotExist()
        {
            _collectionApi.ThrowErrorsAsExceptions = true;
            var body = new PutCollectionPropertyBody
            {
                JournalSize = 313136,
                WaitForSync = false
            };
            var exception = await Assert.ThrowsAsync<ApiErrorException>(async () => await _collectionApi.PutCollectionPropertyAsync("bogusCollection", body));

            Assert.Equal(HttpStatusCode.NotFound, exception.ResponseDetails.Code);
            Assert.Equal(1203, exception.ResponseDetails.ErrorNum); // ARANGO_DATA_SOURCE_NOT_FOUND
        }

        [Fact]
        public async Task PutCollectionPropertyAsync_ShouldReturnError_WhenCollectionDoesNotExist()
        {
            var body = new PutCollectionPropertyBody
            {
                JournalSize = 313136,
                WaitForSync = false
            };
            PutCollectionPropertyResponse putCollectionPropertyResponse = await _collectionApi.PutCollectionPropertyAsync("bogusCollection", body);

            Assert.False(putCollectionPropertyResponse.IsSuccess);
            Assert.True(putCollectionPropertyResponse.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, putCollectionPropertyResponse.ResponseDetails.Code);
            Assert.Equal(1203, putCollectionPropertyResponse.ResponseDetails.ErrorNum); // ARANGO_DATA_SOURCE_NOT_FOUND
        }

        [Fact]
        public async Task GetCollectionFiguresAsync_ShouldSucceed()
        {
            var response = await _collectionApi.GetCollectionFiguresAsync(_testCollection);
            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);
            Assert.NotNull(response.Figures);
        }

        [Fact]
        public async Task GetCollectionFiguresAsync_ShouldThrow_WhenCollectionNotFound()
        {
            _collectionApi.ThrowErrorsAsExceptions = true;
            var exception = await Assert.ThrowsAsync<ApiErrorException>(async () => await _collectionApi.GetCollectionFiguresAsync("bogusCollection"));
            Assert.Equal(HttpStatusCode.NotFound, exception.ResponseDetails.Code);
        }

        [Fact]
        public async Task GetCollectionFiguresAsync_ShouldReturnError_WhenCollectionNotFound()
        {
            GetCollectionFiguresResponse getCollectionFiguresResponse = await _collectionApi.GetCollectionFiguresAsync("bogusCollection");

            Assert.False(getCollectionFiguresResponse.IsSuccess);
            Assert.True(getCollectionFiguresResponse.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.NotFound, getCollectionFiguresResponse.ResponseDetails.Code);
        }
    }
}
