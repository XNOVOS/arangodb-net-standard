using ArangoDBNetStandard.Serialization;
using ArangoDBNetStandard.Transport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ArangoDBNetStandard.CollectionApi.Models;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard
{
    public abstract class ApiClientBase
    {
        protected IApiClientTransport Transport { get; }
        protected IApiClientSerialization Serialization { get; }
        protected abstract string ApiRootPath { get; }
        public bool ThrowErrorsAsExceptions { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="ApiClientBase"/> using
        /// the provided serialization layer.
        /// </summary>
        /// <param name="serialization"></param>
        public ApiClientBase(IApiClientTransport transport, IApiClientSerialization serialization)
        {
            Serialization = serialization;
            Transport = transport;
        }

        protected async Task<ApiResponse> GetApiErrorResponse(IApiClientResponse response)
        {
            var stream = await response.Content.ReadAsStreamAsync();
            var apiErrorResponse = Serialization.DeserializeFromStream<ApiResponse>(stream);
            if (apiErrorResponse == null)
            {
                apiErrorResponse = new ApiResponse(true, response.StatusCode, null, null);
            }
            if (ThrowErrorsAsExceptions)
                throw new ApiErrorException(apiErrorResponse);
            return apiErrorResponse;
        }

        protected async Task<ApiErrorException> GetApiErrorException(IApiClientResponse response)
        {
            var stream = await response.Content.ReadAsStreamAsync();
            var apiErrorResponse = Serialization.DeserializeFromStream<ApiResponse>(stream);
            return new ApiErrorException(apiErrorResponse);
        }

        protected void ValidateDocumentId(string documentId)
        {
            if (documentId.Split('/').Length != 2)
            {
                throw new ArgumentException("A valid document ID has two parts, split by '/'. + " +
                    "" + documentId + " is not a valid document ID. Maybe the document key was used by mistake?");
            }
        }

        protected async Task<TResponse> GetRequestAsync<TResponse>(string uri,
            Func<ApiResponse, TResponse> errorResponseFunc, RequestOptionsBase options = null,
            CancellationToken cancellationToken = default)
        {
            uri = ModifyUriForOptionsValues(uri, options);

            return await DoRequestAsync(() => Transport.GetAsync(uri, cancellationToken), errorResponseFunc);
        }

        private string ModifyUriForOptionsValues(string uri, RequestOptionsBase options)
        {
            if (uri.Last() == '/')
                uri = uri.Substring(0, uri.Length - 1);
            IReadOnlyDictionary<string, string> queryStringValues = options?.ToQueryStringValues();
            if (queryStringValues != null && queryStringValues.Any())
            {
                uri = uri + "?" + string.Join("&",
                          queryStringValues.Select(x => $"{x.Key}={WebUtility.UrlEncode(x.Value.ToLowerInvariant())}"));
            }

            return uri;
        }

        protected async Task<TResponse> DeleteRequestAsync<TResponse>(string uri,
            Func<ApiResponse, TResponse> errorResponseFunc, RequestOptionsBase options = null,
            CancellationToken cancellationToken = default)
        {
            uri = ModifyUriForOptionsValues(uri, options);

            return await DoRequestAsync(() => Transport.DeleteAsync(uri, cancellationToken), errorResponseFunc);
        }

        protected async Task<TResponse> DeleteRequestAsync<TResponse>(string uri,
            Func<ApiResponse, TResponse> errorResponseFunc, RequestOptionsBase options = null,
            object content = null, CancellationToken cancellationToken = default)
        {
            uri = ModifyUriForOptionsValues(uri, options);
            byte[] contentBytes = content != null ? GetContent(content, options?.ContentSerializationOptions?.CamelCasePropertyNames ?? true, options?.ContentSerializationOptions?.IgnoreNullValues ?? true) : new byte[0];
            return await DoRequestAsync(() => Transport.DeleteAsync(uri, contentBytes, cancellationToken), errorResponseFunc);
        }

        protected async Task<TResponse> PostRequestAsync<TResponse>(string uri,
            Func<ApiResponse, TResponse> errorResponseFunc, object content = null,
            RequestOptionsBase options = null, CancellationToken cancellationToken = default)
        {
            uri = ModifyUriForOptionsValues(uri, options);

            byte[] contentBytes = content != null ? GetContent(content, options?.ContentSerializationOptions?.CamelCasePropertyNames ?? true, options?.ContentSerializationOptions?.IgnoreNullValues ?? true) : new byte[0];
            return await DoRequestAsync(() => Transport.PostAsync(uri, contentBytes, cancellationToken), errorResponseFunc);
        }

        protected async Task<TResponse> PatchRequestAsync<TResponse>(string uri,
            Func<ApiResponse, TResponse> errorResponseFunc, object content = null,
            RequestOptionsBase options = null, CancellationToken cancellationToken = default)
        {
            uri = ModifyUriForOptionsValues(uri, options);

            byte[] contentBytes = content != null ? GetContent(content, options?.ContentSerializationOptions?.CamelCasePropertyNames ?? true, options?.ContentSerializationOptions?.IgnoreNullValues ?? true) : new byte[0];
            return await DoRequestAsync(() => Transport.PatchAsync(uri, contentBytes, cancellationToken), errorResponseFunc);
        }

        protected async Task<TResponse> PutRequestAsync<TResponse>(string uri,
            Func<ApiResponse, TResponse> errorResponseFunc, object content = null,
            RequestOptionsBase options = null, CancellationToken cancellationToken = default)
        {
            uri = ModifyUriForOptionsValues(uri, options);

            byte[] contentBytes = content != null ? GetContent(content, options?.ContentSerializationOptions?.CamelCasePropertyNames ?? true, options?.ContentSerializationOptions?.IgnoreNullValues ?? true) : new byte[0];
            return await DoRequestAsync(() => Transport.PutAsync(uri, contentBytes, cancellationToken), errorResponseFunc);
        }

        protected async Task<TResponse> DoRequestAsync<TResponse>(Func<Task<IApiClientResponse>> requestFunc,
            Func<ApiResponse, TResponse> errorResponseFunc)
        {
            using (var response = await requestFunc())
            {
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    return DeserializeJsonFromStream<TResponse>(stream);
                }

                return errorResponseFunc(await GetApiErrorResponse(response));
            }
        }

        protected T DeserializeJsonFromStream<T>(Stream stream)
        {
            return Serialization.DeserializeFromStream<T>(stream);
        }

        protected byte[] GetContent(object item, bool useCamelCasePropertyNames, bool ignoreNullValues)
        {
            return Serialization.Serialize(
                item,
                useCamelCasePropertyNames,
                ignoreNullValues);
        }
    }
}
