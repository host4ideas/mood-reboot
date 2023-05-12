using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace NugetMoodReboot.Helpers
{
    public class HelperApi
    {
        private readonly string _urlApi;
        private IHttpClientFactory HttpClientFactory { get; }

        public HelperApi(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this._urlApi = configuration.GetConnectionString("ApiMoodReboot");
            this.HttpClientFactory = httpClientFactory;
        }

        public async Task<T?> GetAsync<T>(string request)
        {
            using HttpClient httpClient = HttpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(this._urlApi);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await httpClient.GetFromJsonAsync<T>(new Uri(request));
        }

        public async Task<T?> GetAsync<T>(string request, List<KeyValuePair<string, IEnumerable<string>>> additionalHeaders)
        {
            using HttpClient httpClient = HttpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(this._urlApi);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (additionalHeaders != null)
            {
                foreach (var header in additionalHeaders)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            return await httpClient.GetFromJsonAsync<T>(new Uri(request));
        }

        public async Task<HttpResponseMessage> DeleteAsync(string request)
        {
            using HttpClient httpClient = HttpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(this._urlApi);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await httpClient.DeleteAsync(request);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string request, List<KeyValuePair<string, IEnumerable<string>>> additionalHeaders)
        {
            using HttpClient httpClient = HttpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(this._urlApi);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            foreach (var header in additionalHeaders)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            return await httpClient.DeleteAsync(request);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string request, T? body)
        {
            using HttpClient httpClient = HttpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(this._urlApi);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (body == null)
            {
                return await httpClient.PutAsync(request, null);
            }
            return await httpClient.PutAsJsonAsync(request, body);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string request, T? body, List<KeyValuePair<string, IEnumerable<string>>> additionalHeaders)
        {
            using HttpClient httpClient = HttpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(this._urlApi);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (additionalHeaders != null)
            {
                foreach (var header in additionalHeaders)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            if (body == null)
            {
                return await httpClient.PutAsync(request, null);
            }
            return await httpClient.PutAsJsonAsync(request, body);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string request, T? body)
        {
            using HttpClient httpClient = HttpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(this._urlApi);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (body == null)
            {
                return await httpClient.PostAsync(request, null);
            }
            return await httpClient.PostAsJsonAsync(request, body);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string request, T? body, List<KeyValuePair<string, IEnumerable<string>>> additionalHeaders)
        {
            using HttpClient httpClient = HttpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(this._urlApi);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (additionalHeaders != null)
            {
                foreach (var header in additionalHeaders)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            if (body == null)
            {
                return await httpClient.PostAsync(request, null);
            }
            return await httpClient.PostAsJsonAsync(request, body);
        }
    }
}
