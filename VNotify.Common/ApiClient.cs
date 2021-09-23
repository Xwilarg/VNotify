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

        /// <param name="page">Starts at 0 and must be incremented by one until it returns empty result</param>
        public async Task<Channel[]> GetChannelsAsync(int page)
        {
            return await DoApiCallAsync<Channel[]>("https://holodex.net/api/v2/channels?type=vtuber&limit=100&offset=" + (page * 100));
        }

        private readonly HttpClient _httpClient;
    }
}
