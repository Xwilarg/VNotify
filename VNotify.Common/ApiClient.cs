using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace VNotify.Common
{
    public class ApiClient
    {
        public ApiClient(string apiKey)
        {
            _httpClient = new();
            _httpClient.DefaultRequestHeaders.Add("X-APIKEY", apiKey);
        }

        private async Task<T> DoApiCallAsync<T>(string url)
        {
            return JsonSerializer.Deserialize<T>(await _httpClient.GetStringAsync(url));
        }

        public async Task<Video[]> GetLatestStreams()
        {
            return await DoApiCallAsync<Video[]>("https://holodex.net/api/v2/videos?status=upcoming&type=stream&order=asc&include=live_info");
        }

        private HttpClient _httpClient;
    }
}
