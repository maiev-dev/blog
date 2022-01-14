using DatabaseApi.Model;
using System.Text.Json;
using DatabaseApi.Model.Entities;

namespace Front
{
    internal class ApiClient
    {
        private string? _apiKey = null;
        private HttpClient _httpClient;
        public bool IsAuthorized => _apiKey != null;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void Authorize(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentNullException(nameof(apiKey), "Was null");
            _apiKey = apiKey;
        }

        public async Task Register()
        {
            var response = await _httpClient.GetAsync("https://localhost:7166/api/ApiUsers/create");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var apiUser = JsonSerializer.Deserialize<ApiUser>(json);
            _apiKey = apiUser?.ApiKey;
        }

        public async Task PostArticle(Article article)
        {
            var body = $"\"header\": \"{article.Header}\"," +
                         $"\"abstract\": \"{article.Abstract}\"," +
                        $"\"text\": \"{article.Text}\"";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7166/api/Articles/create?apiKey=" + _apiKey),
                Content = new StringContent(body)
            };
            await _httpClient.SendAsync(request);
        }

    }
}
