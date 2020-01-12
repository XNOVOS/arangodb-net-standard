using ArangoDBNetStandard.DatabaseApi.Models;
using ArangoDBNetStandard.Serialization;
using ArangoDBNetStandard.Transport;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ArangoDBNetStandard.DatabaseApi
{
    public class DatabaseApiClient : ApiClientBase, IDatabaseApiClient
    {
        protected override string ApiRootPath => "_api/database";

        /// <summary>
        /// Creates an instance of <see cref="DatabaseApiClient"/>
        /// using the provided transport layer and the default JSON serialization.
        /// </summary>
        /// <param name="client"></param>
        public DatabaseApiClient(IApiClientTransport transport)
            : base(transport, new JsonNetApiClientSerialization())
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="DatabaseApiClient"/>
        /// using the provided transport and serialization layers.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="serializer"></param>
        public DatabaseApiClient(IApiClientTransport transport, IApiClientSerialization serializer)
            : base(transport, serializer)
        {
        }

        /// <summary>
        /// Creates a new database.
        /// (Only possible from within the _system database)
        /// </summary>
        /// <param name="request">The parameters required by this endpoint.</param>
        /// <returns></returns>
        public virtual async Task<PostDatabaseResponse> PostDatabaseAsync(PostDatabaseBody request, CancellationToken cancellationToken = default)
        {
            return await PostRequestAsync(ApiRootPath, response => new PostDatabaseResponse(response), request, null,
                cancellationToken);
        }

        /// <summary>
        /// Delete a database. Dropping a database is only possible from within the _system database.
        /// The _system database itself cannot be dropped.
        /// DELETE /_api/database/{database-name}
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public virtual async Task<DeleteDatabaseResponse> DeleteDatabaseAsync(string databaseName, CancellationToken cancellationToken = default)
        {
            return await DeleteRequestAsync($"{ApiRootPath}/{WebUtility.UrlEncode(databaseName)}",
                response => new DeleteDatabaseResponse(response), null, cancellationToken);
        }

        /// <summary>
        /// Retrieves the list of all existing databases.
        /// (Only possible from within the _system database)
        /// </summary>
        /// <remarks>
        /// You should use <see cref="GetUserDatabasesAsync"/> to fetch the list of the databases
        /// available for the current user.
        /// </remarks>
        /// <returns></returns>
        public virtual async Task<GetDatabasesResponse> GetDatabasesAsync(CancellationToken cancellationToken = default)
        {
            return await GetRequestAsync(ApiRootPath, response => new GetDatabasesResponse(response), null,
                cancellationToken);
        }

        /// <summary>
        /// Retrieves the list of all databases the current user can access.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<GetDatabasesResponse> GetUserDatabasesAsync(CancellationToken cancellationToken = default)
        {
            return await GetRequestAsync(ApiRootPath + "/user", response => new GetDatabasesResponse(response), null,
                cancellationToken);
        }

        /// <summary>
        /// Retrieves information about the current database.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<GetCurrentDatabaseInfoResponse> GetCurrentDatabaseInfoAsync(
            CancellationToken cancellationToken = default)
        {
            return await GetRequestAsync(ApiRootPath + "/current",
                response => new GetCurrentDatabaseInfoResponse(response), null, cancellationToken);
        }
    }
}
