using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ArangoDBNetStandard.Transport.Http
{
    public class HttpApiClientResponse : IApiClientResponse
    {
        private readonly HttpResponseMessage _response;

        public HttpApiClientResponse(HttpResponseMessage response)
        {
            this._response = response;
            Headers = response.Headers;
            Content = new HttpApiClientResponseContent(response.Content);
            IsSuccessStatusCode = response.IsSuccessStatusCode;
            StatusCode = response.StatusCode;

        }

        public IApiClientResponseContent Content { get; }

        public bool IsSuccessStatusCode { get; }

        public HttpStatusCode StatusCode { get; }

        public HttpResponseHeaders Headers { get; set; }

        public void Dispose()
        {
            _response.Dispose();
        }
    }
}
