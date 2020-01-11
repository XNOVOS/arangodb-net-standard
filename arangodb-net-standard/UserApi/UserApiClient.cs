using System.Net;
using System.Threading.Tasks;

using ArangoDBNetStandard.Serialization;
using ArangoDBNetStandard.Transport;
using ArangoDBNetStandard.UserApi.Models;

namespace ArangoDBNetStandard.UserApi
{
    public class UserApiClient : ApiClientBase, IUserApiClient
    {
        protected override string ApiRootPath => "_api/user";

        public UserApiClient(IApiClientTransport transport)
            : base(transport, new JsonNetApiClientSerialization())
        {
        }

        public UserApiClient(IApiClientTransport transport, IApiClientSerialization serializer)
            : base(transport, serializer)
        {
        }

        public async Task<DeleteUserResponse> DeleteUserAsync(string username)
        {
            string uri = ApiRootPath + "/" + WebUtility.HtmlEncode(username);
            using (var response = await Transport.DeleteAsync(uri))
            {
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    return DeserializeJsonFromStream<DeleteUserResponse>(stream);
                }
                throw await GetApiErrorException(response);
            }
        }
    }
}
