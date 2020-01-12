﻿using ArangoDBNetStandard;
using ArangoDBNetStandard.DatabaseApi;
using ArangoDBNetStandard.DatabaseApi.Models;
using ArangoDBNetStandard.Transport.Http;
using ArangoDBNetStandard.UserApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArangoDBNetStandard.Serialization;
using ArangoDBNetStandardTest.AuthApi;
using Xunit;

namespace ArangoDBNetStandardTest
{
    public abstract class ApiClientTestFixtureBase : IDisposable, IAsyncLifetime
    {
        private readonly List<string> _databases = new List<string>();
        private readonly List<string> _users = new List<string>();

        private readonly List<HttpApiTransport> _transports = new List<HttpApiTransport>();

        public string ArangoDbHost => Environment.GetEnvironmentVariable("ARANGO_HOST") ?? ArangoDBTestSettings.ArangoHost;

        protected ApiClientTestFixtureBase()
        {
            //int listenersCount = System.Diagnostics.Trace.Listeners.Count;
        }

        private IApiClientSerialization GetApiClientSerialization()
        {
            return ArangoDBTestSettings.EnableJsonSerializationTracing
                ? new JsonNetApiClientSerializationWithTracing()
                : new JsonNetApiClientSerialization();
        }

        protected HttpApiTransport GetHttpTransport(string dbName)
        {
            var transport = HttpApiTransport.UsingBasicAuth(
                new Uri($"http://{ArangoDbHost}:8529/"),
                dbName,
                ArangoDBTestSettings.RootUsername,
                ArangoDBTestSettings.RootPassword);
            _transports.Add(transport);
            return transport;
        }

        protected ArangoDBClient GetArangoDBClient(string dbName)
        {
            var httpTransport = GetHttpTransport(dbName);
            return new ArangoDBClient(httpTransport, GetApiClientSerialization());
        }

        /// <summary>
        /// Databases and users created through this method will be dropped
        /// during the test fixture's dispose routine. For that reason, do not pass details
        /// of an existing user or database that you expect to stay around after a test run.
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="users">Optional set of users to create along with the database.</param>
        /// <returns></returns>
        protected async Task CreateDatabase(string dbName, IEnumerable<DatabaseUser> users = null)
        {
            // Create the test database
            using (var systemDbClient = GetHttpTransport("_system"))
            {
                var dbApiClient = new DatabaseApiClient(systemDbClient, GetApiClientSerialization());
                try
                {
                    await dbApiClient.PostDatabaseAsync(new PostDatabaseBody
                    {
                        Name = dbName,
                        Users = users
                    });
                }
                catch (ApiErrorException ex) when (ex.ResponseDetails.ErrorNum == 1207)
                {
                    // database must exist already
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    _databases.Add(dbName);
                    if (users != null)
                    {
                        _users?.AddRange(users.Select(u => u.Username));
                    }
                }
            }
        }

        public virtual void Dispose()
        {
            foreach (var transport in _transports)
            {
                try
                {
                    transport.Dispose();
                }
                catch (ObjectDisposedException)
                {
                    continue;
                }
            }
        }

        public virtual Task InitializeAsync()
        {
            return Task.FromResult(0);
        }

        private async Task DropUserAsync(string user)
        {
            using (var systemDbClient = GetHttpTransport("_system"))
            {
                var userApiClient = new UserApiClient(systemDbClient, GetApiClientSerialization());
                await userApiClient.DeleteUserAsync(user);
            }
        }

        protected async Task DropDatabase(string dbName)
        {
            using (var systemDbClient = GetHttpTransport("_system"))
            {
                var dbApiClient = new DatabaseApiClient(systemDbClient, GetApiClientSerialization());
                var response = await dbApiClient.DeleteDatabaseAsync(dbName);
            }
        }

        public virtual async Task DisposeAsync()
        {
            foreach (var dbName in _databases)
            {
                try
                {
                    await DropDatabase(dbName);
                }
                catch (ApiErrorException ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }

            foreach (var user in _users)
            {
                try
                {
                    await DropUserAsync(user);
                }
                catch (ApiErrorException ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }
        }

    }
}
