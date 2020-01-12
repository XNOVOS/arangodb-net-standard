using System.Net;
using System.Threading;
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

        public async Task<DeleteUserResponse> DeleteUserAsync(string username, CancellationToken cancellationToken = default)
        {
            return await DeleteRequestAsync($"{ApiRootPath}/{WebUtility.HtmlEncode(username)}",
                response => new DeleteUserResponse(response), null, cancellationToken);
        }
    }
}
