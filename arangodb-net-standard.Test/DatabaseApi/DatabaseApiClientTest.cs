using ArangoDBNetStandard;
using ArangoDBNetStandard.DatabaseApi;
using ArangoDBNetStandard.DatabaseApi.Models;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ArangoDBNetStandardTest.DatabaseApi
{
    /// <summary>
    /// Test class for <see cref="DatabaseApiClient"/>.
    /// </summary>
    public class DatabaseApiClientTest : IClassFixture<DatabaseApiClientTestFixture>
    {
        private readonly DatabaseApiClientTestFixture _fixture;
        private readonly DatabaseApiClient _systemClient;

        public DatabaseApiClientTest(DatabaseApiClientTestFixture fixture)
        {
            _fixture = fixture;
            _systemClient = fixture.DatabaseClientSystem;
            _systemClient.ThrowErrorsAsExceptions = false;
            _fixture.DatabaseClientNonExistent.ThrowErrorsAsExceptions = false;
            _fixture.DatabaseClientOther.ThrowErrorsAsExceptions = false;
        }

        [Fact]
        public async Task PostDatabaseAsync_ShouldSucceed()
        {
            PostDatabaseResponse result = await _fixture.DatabaseClientSystem.PostDatabaseAsync(
                new PostDatabaseBody
                {
                    Name = nameof(PostDatabaseAsync_ShouldSucceed)
                });

            await _fixture.DatabaseClientSystem.DeleteDatabaseAsync(nameof(PostDatabaseAsync_ShouldSucceed));

            Assert.False(result.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.Created, result.ResponseDetails.Code);
            Assert.True(result.Result);
        }

        [Fact]
        public async Task PostDatabaseAsync_ShouldThrow_WhenDatabaseUsedIsNotSystem()
        {
            _fixture.DatabaseClientOther.ThrowErrorsAsExceptions = true;
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _fixture.DatabaseClientOther.PostDatabaseAsync(new PostDatabaseBody
                {
                    Name = nameof(PostDatabaseAsync_ShouldThrow_WhenDatabaseUsedIsNotSystem)
                });
            });

            ApiResponse apiError = ex.ApiError;

            Assert.Equal(HttpStatusCode.Forbidden, apiError.Code);
            Assert.Equal(1230, apiError.ErrorNum);
        }

        [Fact]
        public async Task PostDatabaseAsync_ShouldReturnError_WhenDatabaseUsedIsNotSystem()
        {
            PostDatabaseResponse postDatabaseResponse = await _fixture.DatabaseClientOther.PostDatabaseAsync(new PostDatabaseBody
            {
                Name = nameof(PostDatabaseAsync_ShouldThrow_WhenDatabaseUsedIsNotSystem)
            });

            Assert.False(postDatabaseResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.Forbidden, postDatabaseResponse.ResponseDetails.Code);
            Assert.Equal(1230,postDatabaseResponse.ResponseDetails.ErrorNum);
        }

        [Fact]
        public async Task PostDatabaseAsync_ShouldThrow_WhenDatabaseToCreateAlreadyExist()
        {
            _systemClient.ThrowErrorsAsExceptions = true;
            await _fixture.DatabaseClientSystem.PostDatabaseAsync(new PostDatabaseBody
            {
                Name = nameof(PostDatabaseAsync_ShouldThrow_WhenDatabaseToCreateAlreadyExist)
            });

            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _fixture.DatabaseClientSystem.PostDatabaseAsync(new PostDatabaseBody
                {
                    Name = nameof(PostDatabaseAsync_ShouldThrow_WhenDatabaseToCreateAlreadyExist)
                });
            });

            await _fixture.DatabaseClientSystem.DeleteDatabaseAsync(
                nameof(PostDatabaseAsync_ShouldThrow_WhenDatabaseToCreateAlreadyExist));

            ApiResponse apiError = ex.ApiError;

            Assert.Equal(HttpStatusCode.Conflict, apiError.Code);
            Assert.Equal(1207, apiError.ErrorNum);
        }

        [Fact]
        public async Task PostDatabaseAsync_ShouldReturnError_WhenDatabaseToCreateAlreadyExist()
        {
            await _fixture.DatabaseClientSystem.PostDatabaseAsync(new PostDatabaseBody
            {
                Name = nameof(PostDatabaseAsync_ShouldThrow_WhenDatabaseToCreateAlreadyExist)
            });

            PostDatabaseResponse postDatabaseResponse = await _fixture.DatabaseClientSystem.PostDatabaseAsync(new PostDatabaseBody
            {
                Name = nameof(PostDatabaseAsync_ShouldThrow_WhenDatabaseToCreateAlreadyExist)
            });

            await _fixture.DatabaseClientSystem.DeleteDatabaseAsync(
                nameof(PostDatabaseAsync_ShouldThrow_WhenDatabaseToCreateAlreadyExist));

            Assert.False(postDatabaseResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.Conflict, postDatabaseResponse.ResponseDetails.Code);
            Assert.Equal(1207, postDatabaseResponse.ResponseDetails.ErrorNum);
        }

        [Fact]
        public async Task PostDatabaseAsync_ShouldThrow_WhenDatabaseUsedDoesNotExist()
        {
            _fixture.DatabaseClientNonExistent.ThrowErrorsAsExceptions = true;
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _fixture.DatabaseClientNonExistent.PostDatabaseAsync(new PostDatabaseBody
                {
                    Name = nameof(PostDatabaseAsync_ShouldThrow_WhenDatabaseUsedDoesNotExist)
                });
            });

            ApiResponse apiError = ex.ApiError;

            Assert.Equal(HttpStatusCode.NotFound, apiError.Code);
            Assert.Equal(1228, apiError.ErrorNum);
        }

        [Fact]
        public async Task PostDatabaseAsync_ShouldReturnError_WhenDatabaseUsedDoesNotExist()
        {
            PostDatabaseResponse postDatabaseResponse = await _fixture.DatabaseClientNonExistent.PostDatabaseAsync(new PostDatabaseBody
            {
                Name = nameof(PostDatabaseAsync_ShouldThrow_WhenDatabaseUsedDoesNotExist)
            });

            Assert.False(postDatabaseResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, postDatabaseResponse.ResponseDetails.Code);
            Assert.Equal(1228, postDatabaseResponse.ResponseDetails.ErrorNum);
        }

        [Fact]
        public async Task ListDatabasesAsync_ShouldSucceed()
        {
            GetDatabasesResponse result = await _fixture.DatabaseClientSystem.GetDatabasesAsync();

            Assert.False(result.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.OK, result.ResponseDetails.Code);
            Assert.True(result.Results.Count > 0);
        }

        [Fact]
        public async Task ListDatabasesAsync_ShouldThrow_WhenDatabaseIsNotSystem()
        {
            _fixture.DatabaseClientOther.ThrowErrorsAsExceptions = true;
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _fixture.DatabaseClientOther.GetDatabasesAsync();
            });

            ApiResponse apiError = ex.ApiError;

            Assert.Equal(HttpStatusCode.Forbidden, apiError.Code);
            Assert.Equal(1230, apiError.ErrorNum);
        }

        [Fact]
        public async Task ListDatabasesAsync_ShouldReturnError_WhenDatabaseIsNotSystem()
        {
            GetDatabasesResponse getDatabasesResponse = await _fixture.DatabaseClientOther.GetDatabasesAsync();

            Assert.False(getDatabasesResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.Forbidden, getDatabasesResponse.ResponseDetails.Code);
            Assert.Equal(1230, getDatabasesResponse.ResponseDetails.ErrorNum);
        }

        [Fact]
        public async Task ListDatabasesAsync_ShouldThrow_WhenDatabaseDoesNotExist()
        {
            _systemClient.ThrowErrorsAsExceptions = true;
            GetDatabasesResponse getDatabasesResponse = await _fixture.DatabaseClientNonExistent.GetDatabasesAsync();

            Assert.False(getDatabasesResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, getDatabasesResponse.ResponseDetails.Code);
            Assert.Equal(1228, getDatabasesResponse.ResponseDetails.ErrorNum);
        }

        [Fact]
        public async Task ListUserDatabasesAsync_ShouldSucceed()
        {
            GetDatabasesResponse result = await _fixture.DatabaseClientOther.GetUserDatabasesAsync();

            Assert.False(result.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.OK, result.ResponseDetails.Code);
            Assert.True(result.Results.Count > 0);
        }

        [Fact]
        public async Task ListUserDatabasesAsync_ShouldThrow_WhenDatabaseDoesNotExist()
        {
            _fixture.DatabaseClientNonExistent.ThrowErrorsAsExceptions = true;
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _fixture.DatabaseClientNonExistent.GetDatabasesAsync();
            });

            ApiResponse apiError = ex.ApiError;

            Assert.Equal(HttpStatusCode.NotFound, apiError.Code);
            Assert.Equal(1228, apiError.ErrorNum);
        }

        [Fact]
        public async Task ListUserDatabasesAsync_ShouldReturnError_WhenDatabaseDoesNotExist()
        {
            GetDatabasesResponse getDatabasesResponse = await _fixture.DatabaseClientNonExistent.GetDatabasesAsync();

            Assert.False(getDatabasesResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, getDatabasesResponse.ResponseDetails.Code);
            Assert.Equal(1228, getDatabasesResponse.ResponseDetails.ErrorNum);
        }

        [Fact]
        public async Task GetCurrentDatabaseInfoAsync_ShouldSucceed()
        {
            GetCurrentDatabaseInfoResponse response =
                await _fixture.DatabaseClientSystem.GetCurrentDatabaseInfoAsync();

            Assert.False(response.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);

            CurrentDatabaseInfo dbInfo = response.Result;

            Assert.NotNull(dbInfo);
            Assert.NotNull(dbInfo.Id);
            Assert.True(dbInfo.IsSystem);
            Assert.Equal("_system", dbInfo.Name);
            Assert.NotNull(dbInfo.Path);
        }

        [Fact]
        public async Task GetCurrentDatabaseInfoAsync_ShouldThrow_WhenDatabaseDoesNotExist()
        {
            _fixture.DatabaseClientNonExistent.ThrowErrorsAsExceptions = true;
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _fixture.DatabaseClientNonExistent.GetDatabasesAsync();
            });

            ApiResponse apiError = ex.ApiError;

            Assert.Equal(HttpStatusCode.NotFound, apiError.Code);
            Assert.Equal(1228, apiError.ErrorNum);
        }

        [Fact]
        public async Task GetCurrentDatabaseInfoAsync_ShouldReturnError_WhenDatabaseDoesNotExist()
        {
            GetDatabasesResponse getDatabasesResponse = await _fixture.DatabaseClientNonExistent.GetDatabasesAsync();

            Assert.False(getDatabasesResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, getDatabasesResponse.ResponseDetails.Code);
            Assert.Equal(1228, getDatabasesResponse.ResponseDetails.ErrorNum);
        }

        [Fact]
        public async Task DeleteDatabaseAsync_ShouldSucceed()
        {
            var response = await _systemClient.DeleteDatabaseAsync(_fixture.DeletableDatabase);
            Assert.Equal(HttpStatusCode.OK, response.ResponseDetails.Code);
            Assert.False(response.ResponseDetails.Error);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task DeleteDatabaseAsync_ShouldThrow_WhenDatabaseDoesNotExist()
        {
            _systemClient.ThrowErrorsAsExceptions = true;
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _systemClient.DeleteDatabaseAsync("bogusDatabase");
            });
            Assert.Equal(HttpStatusCode.NotFound, ex.ApiError.Code);
            Assert.Equal(1228, ex.ApiError.ErrorNum); // ARANGO_DATABASE_NOT_FOUND
        }

        [Fact]
        public async Task DeleteDatabaseAsync_ShouldReturnError_WhenDatabaseDoesNotExist()
        {
            DeleteDatabaseResponse deleteDatabaseResponse = await _systemClient.DeleteDatabaseAsync("bogusDatabase");

            Assert.False(deleteDatabaseResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, deleteDatabaseResponse.ResponseDetails.Code);
            Assert.Equal(1228, deleteDatabaseResponse.ResponseDetails.ErrorNum); // ARANGO_DATABASE_NOT_FOUND
        }
    }
}
