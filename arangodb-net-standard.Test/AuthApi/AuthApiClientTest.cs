using ArangoDBNetStandard;
using ArangoDBNetStandard.AuthApi.Models;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ArangoDBNetStandardTest.AuthApi
{
    public class AuthApiClientTest: IClassFixture<AuthApiClientTestFixture>
    {
        private readonly AuthApiClientTestFixture _fixture;

        public AuthApiClientTest(AuthApiClientTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.ArangoDBClient.Auth.ThrowErrorsAsExceptions = false;
        }

        [Fact]
        public async Task GetJwtToken_ShouldSucceed()
        {
            JwtTokenResponse response = await _fixture.ArangoDBClient.Auth.GetJwtTokenAsync(
                new JwtTokenRequestBody
                {
                    Username = _fixture.Username,
                    Password = _fixture.Password
                });
            Assert.NotNull(response.Jwt);
        }

        [Fact]
        public async Task GetJwtToken_ShouldThrow_WhenWrongCredentialsUsed()
        { 
            var ex = await Assert.ThrowsAsync<ApiErrorException>(async () =>
            {
                _fixture.ArangoDBClient.Auth.ThrowErrorsAsExceptions = true;

                await _fixture.ArangoDBClient.Auth.GetJwtTokenAsync(
                    new JwtTokenRequestBody
                    {
                        Username = _fixture.Username,
                        Password = "wrongpw"
                    });
            });
            Assert.NotNull(ex.ApiError);
            Assert.True(ex.ApiError.Error);
            Assert.Equal(HttpStatusCode.Unauthorized, ex.ApiError.Code);
            Assert.Equal(401, ex.ApiError.ErrorNum);
            Assert.NotNull(ex.ApiError.ErrorMessage);
        }

        [Fact]
        public async Task GetJwtToken_ShouldReturnError_WhenWrongCredentialsUsed()
        {
            JwtTokenResponse jwtTokenResponse = await _fixture.ArangoDBClient.Auth.GetJwtTokenAsync(
                new JwtTokenRequestBody
                {
                    Username = _fixture.Username,
                    Password = "wrongpw"
                });

            Assert.False(jwtTokenResponse.IsSuccess);
            Assert.NotNull(jwtTokenResponse.ResponseDetails);
            Assert.True(jwtTokenResponse.ResponseDetails.Error);
            Assert.Equal(HttpStatusCode.Unauthorized, jwtTokenResponse.ResponseDetails.Code);
            Assert.Equal(401, jwtTokenResponse.ResponseDetails.ErrorNum);
            Assert.NotNull(jwtTokenResponse.ResponseDetails.ErrorMessage);
        }
    }
}
