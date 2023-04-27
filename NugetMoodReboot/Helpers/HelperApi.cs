using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace NugetMoodReboot.Helpers
{
    public class HelperApi
    {
        private readonly string _urlApi;
        private readonly HttpClient _httpClient;

        public HelperApi(string urlApi)
        {
            _urlApi = urlApi;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_urlApi)
            };
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public HelperApi(string urlApi, HttpRequestHeaders headers)
        {
            _urlApi = urlApi;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_urlApi)
            };
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            foreach (var header in headers)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        public async Task<T?> GetAsync<T>(string request)
        {
            return await _httpClient.GetFromJsonAsync<T>(request);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string request)
        {
            return await _httpClient.DeleteAsync(request);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string request, T? body)
        {
            if (body == null)
            {
                return await _httpClient.PutAsync(request, null);
            }
            return await _httpClient.PutAsJsonAsync(request, body);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string request, T? body)
        {
            if (body == null)
            {
                return await _httpClient.PostAsync(request, null);
            }
            return await _httpClient.PostAsJsonAsync(request, body);
        }
    }
}
