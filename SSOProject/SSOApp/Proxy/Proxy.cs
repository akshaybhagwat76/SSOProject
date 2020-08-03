using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SSOApp.Proxy
{
    public interface IAPIClientProxy
    {
        Task<HttpResponseMessage> Send(string url, HttpMethod method, object requestPayload = null);
    }
    public class APIClientProxy : IAPIClientProxy
    {
        private readonly HttpClient _client;

        public APIClientProxy(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> Send(string url, HttpMethod method, object requestPayload = null)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(_client.BaseAddress, url),
                Method = method
            };

            if (requestPayload != null)
            {
                request.Content = new StringContent((string)requestPayload, UnicodeEncoding.UTF8, "application/json");
            }

            var httpResponse = await _client.SendAsync(request);
            return httpResponse;
        }

        public Task<HttpResponseMessage> Send(string url, string method, object requestPayload = null)
        {
            throw new NotImplementedException();
        }
    }

   
}
