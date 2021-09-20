using System.Net.Http;

namespace VNotify.Models
{
    public static class StaticObjects
    {
        public static HttpClient HttpClient { get; } = new();
    }
}
