using ArangoDBNetStandard;
using ArangoDBNetStandard.AqlFunctionApi.Models;
using System.Net;
using System.Threading.Tasks;
using ArangoDBNetStandard.Models;
using Xunit;

namespace ArangoDBNetStandardTest.AqlFunctionApi
{
    /// <summary>
    /// Test class for <see cref="AqlFunctionApiClient"/>.
    /// </summary>
    public class AqlFunctionApiClientTest : IClassFixture<AqlFunctionApiClientTestFixture>
    {
        private readonly AqlFunctionApiClientTestFixture _fixture;

        public AqlFunctionApiClientTest(AqlFunctionApiClientTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.AqlFunctionClient.ThrowErrorsAsExceptions = false;
        }

        [Fact]
        public async Task PostAqlFunctionAsync_ShouldSucceed()
        {
            string fullName = string.Join(
                "::",
                System.Environment.TickCount.ToString(),
                nameof(PostAqlFunctionAsync_ShouldSucceed));

            PostAqlFunctionResponse response = await _fixture.AqlFunctionClient.PostAqlFunctionAsync(
                new PostAqlFunctionBody
                {
                    Name = fullName,
                    Code = "function (celsius) { return celsius * 1.8 + 32; }",
                    IsDeterministic = true
                });

            Assert.False(response.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.Created, response.ResponseDetails.Code);
            Assert.True(response.IsNewlyCreated);
        }

        [Fact]
        public async Task PostAqlFunctionAsync_ShouldThrow_WhenFunctionNameIsInvalid()
        {
            _fixture.AqlFunctionClient.ThrowErrorsAsExceptions = true;
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _fixture.AqlFunctionClient.PostAqlFunctionAsync(
                    new PostAqlFunctionBody()
                    {
                        // A non-fully qualified name will give an error
                        Name = nameof(PostAqlFunctionAsync_ShouldSucceed),
                        Code = "function (celsius) { return celsius * 1.8 + 32; }",
                        IsDeterministic = true
                    });
            });

            ApiResponse apiError = ex.ResponseDetails;

            Assert.Equal(HttpStatusCode.BadRequest, apiError.Code);
            Assert.Equal(1580, apiError.ErrorNum); // ERROR_QUERY_FUNCTION_INVALID_NAME
        }

        [Fact]
        public async Task PostAqlFunctionAsync_ShouldReturnError_WhenFunctionNameIsInvalid()
        {
            PostAqlFunctionResponse postAqlFunctionResponse = await _fixture.AqlFunctionClient.PostAqlFunctionAsync(
                new PostAqlFunctionBody()
                {
                    // A non-fully qualified name will give an error
                    Name = nameof(PostAqlFunctionAsync_ShouldSucceed),
                    Code = "function (celsius) { return celsius * 1.8 + 32; }",
                    IsDeterministic = true
                });

            Assert.False(postAqlFunctionResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.BadRequest, postAqlFunctionResponse.ResponseDetails.Code);
            Assert.Equal(1580, postAqlFunctionResponse.ResponseDetails.ErrorNum); // ERROR_QUERY_FUNCTION_INVALID_NAME
        }

        [Fact]
        public async Task DeleteAqlFunctionAsync_ShouldSucceed()
        {
            string groupName = System.Environment.TickCount.ToString();

            string fullName = string.Join(
                "::",
                groupName,
                nameof(DeleteAqlFunctionAsync_ShouldSucceed));

            PostAqlFunctionResponse postResponse =
                await _fixture.AqlFunctionClient.PostAqlFunctionAsync(
                    new PostAqlFunctionBody()
                    {
                        Name = fullName,
                        Code = "function (celsius) { return celsius * 1.8 + 32; }",
                        IsDeterministic = true
                    });

            DeleteAqlFunctionResponse deleteResponse =
                await _fixture.AqlFunctionClient.DeleteAqlFunctionAsync(
                    groupName,
                    new DeleteAqlFunctionQuery()
                    {
                        Group = true
                    });

            Assert.False(deleteResponse.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.OK, deleteResponse.ResponseDetails.Code);
            Assert.Equal(1, deleteResponse.DeletedCount);
        }

        [Fact]
        public async Task DeleteAqlFunctionAsync_ShouldThrow_WhenFunctionNameIsInvalid()
        {
            _fixture.AqlFunctionClient.ThrowErrorsAsExceptions = true;
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                await _fixture.AqlFunctionClient.DeleteAqlFunctionAsync(
                    "你好",
                    new DeleteAqlFunctionQuery()
                    {
                        Group = true
                    });
            });

            ApiResponse apiError = ex.ResponseDetails;

            Assert.Equal(HttpStatusCode.BadRequest, apiError.Code);
            Assert.Equal(1580, apiError.ErrorNum); // ERROR_QUERY_FUNCTION_INVALID_NAME
        }

        [Fact]
        public async Task DeleteAqlFunctionAsync_ShouldReturnError_WhenFunctionNameIsInvalid()
        {
            DeleteAqlFunctionResponse deleteAqlFunctionResponse = await _fixture.AqlFunctionClient.DeleteAqlFunctionAsync(
                "你好",
                new DeleteAqlFunctionQuery()
                {
                    Group = true
                });

            Assert.False(deleteAqlFunctionResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.BadRequest, deleteAqlFunctionResponse.ResponseDetails.Code);
            Assert.Equal(1580, deleteAqlFunctionResponse.ResponseDetails.ErrorNum); // ERROR_QUERY_FUNCTION_INVALID_NAME
        }

        [Fact]
        public async Task GetAqlFunctionsAsync_ShouldSucceed()
        {
            string groupName = System.Environment.TickCount.ToString();

            string fullName = string.Join(
                "::",
                groupName,
                nameof(DeleteAqlFunctionAsync_ShouldSucceed));

            PostAqlFunctionResponse postResponse =
                await _fixture.AqlFunctionClient.PostAqlFunctionAsync(
                    new PostAqlFunctionBody()
                    {
                        Name = fullName,
                        Code = "function (celsius) { return celsius * 1.8 + 32; }",
                        IsDeterministic = true
                    });

            GetAqlFunctionsResponse getResponse =
                await _fixture.AqlFunctionClient.GetAqlFunctionsAsync(
                    new GetAqlFunctionsQuery()
                    {
                        Namespace = groupName
                    });

            Assert.False(getResponse.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.OK, getResponse.ResponseDetails.Code);
            Assert.Single(getResponse);

            AqlFunctionResult firstResult = getResponse[0];

            Assert.Equal(fullName, firstResult.Name);
            Assert.Equal("function (celsius) { return celsius * 1.8 + 32; }", firstResult.Code);
            Assert.True(firstResult.IsDeterministic);
        }
    }
}
