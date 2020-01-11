﻿using System;
using System.Net.Http;
using ArangoDBNetStandard.AqlFunctionApi;
using ArangoDBNetStandard.AuthApi;
using ArangoDBNetStandard.CollectionApi;
using ArangoDBNetStandard.CursorApi;
using ArangoDBNetStandard.DatabaseApi;
using ArangoDBNetStandard.DocumentApi;
using ArangoDBNetStandard.GraphApi;
using ArangoDBNetStandard.Serialization;
using ArangoDBNetStandard.TransactionApi;
using ArangoDBNetStandard.Transport;
using ArangoDBNetStandard.Transport.Http;

namespace ArangoDBNetStandard
{
    /// <summary>
    /// Wrapper class providing access to the complete set of ArangoDB REST resources.
    /// </summary>
    public class ArangoDBClient : IDisposable
    {
        private readonly IApiClientTransport _transport;

        /// <summary>
        /// AQL user functions management API.
        /// </summary>
        public AqlFunctionApiClient AqlFunction { get; set; }

        public AuthApiClient Auth { get; private set; }

        /// <summary>
        /// Cursor API
        /// </summary>
        public CursorApiClient Cursor { get; private set; }

        /// <summary>
        /// Database API
        /// </summary>
        public DatabaseApiClient Database { get; private set; }

        /// <summary>
        /// Document API
        /// </summary>
        public DocumentApiClient Document { get; private set; }

        /// <summary>
        /// Collection API
        /// </summary>
        public CollectionApiClient Collection { get; private set; }

        /// <summary>
        /// Transaction API
        /// </summary>
        public TransactionApiClient Transaction { get; private set; }

        /// <summary>
        /// Graph API
        /// </summary>
        public GraphApiClient Graph { get; private set; }

        /// <summary>
        /// Create an instance of <see cref="ArangoDBClient"/> from an existing
        /// <see cref="HttpClient"/> instance, using the default JSON serialization.
        /// </summary>
        /// <param name="client"></param>
        public ArangoDBClient(HttpClient client)
        {
            _transport = new HttpApiTransport(client, HttpContentType.Json);
            AqlFunction = new AqlFunctionApiClient(_transport);
            Auth = new AuthApiClient(_transport);
            Cursor = new CursorApiClient(_transport);
            Database = new DatabaseApiClient(_transport);
            Document = new DocumentApiClient(_transport);
            Collection = new CollectionApiClient(_transport);
            Transaction = new TransactionApiClient(_transport);
            Graph = new GraphApiClient(_transport);
        }

        /// <summary>
        /// Create an instance of <see cref="ArangoDBClient"/>
        /// using the provided transport layer and the default JSON serialization.
        /// </summary>
        /// <param name="transport">The ArangoDB transport layer implementation.</param>
        public ArangoDBClient(IApiClientTransport transport)
        {
            _transport = transport;

            var serialization = new JsonNetApiClientSerialization();

            AqlFunction = new AqlFunctionApiClient(_transport, serialization);
            Auth = new AuthApiClient(_transport, serialization);
            Cursor = new CursorApiClient(_transport, serialization);
            Database = new DatabaseApiClient(_transport, serialization);
            Document = new DocumentApiClient(_transport, serialization);
            Collection = new CollectionApiClient(_transport, serialization);
            Transaction = new TransactionApiClient(_transport, serialization);
            Graph = new GraphApiClient(_transport, serialization);
        }

        /// <summary>
        /// Create an instance of <see cref="ArangoDBClient"/>
        /// using the provided transport and serialization layers.
        /// </summary>
        /// <param name="transport">The ArangoDB transport layer implementation.</param>
        /// <param name="serialization">The serialization layer implementation.</param>
        public ArangoDBClient(IApiClientTransport transport, IApiClientSerialization serialization)
        {
            _transport = transport;
            AqlFunction = new AqlFunctionApiClient(_transport, serialization);
            Auth = new AuthApiClient(_transport, serialization);
            Cursor = new CursorApiClient(_transport, serialization);
            Database = new DatabaseApiClient(_transport, serialization);
            Document = new DocumentApiClient(_transport, serialization);
            Collection = new CollectionApiClient(_transport, serialization);
            Transaction = new TransactionApiClient(_transport, serialization);
            Graph = new GraphApiClient(_transport, serialization);
        }

        /// <summary>
        /// Disposes the underlying transport instance.
        /// </summary>
        public void Dispose()
        {
            _transport.Dispose();
        }
    }
}
