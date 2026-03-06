using System.Text;
using System.Text.Json;

namespace Project3Vitour.Services.ModerationServices
{
    public class ModerationService : IModerationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ModerationService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["Anthropic:ApiKey"];
        }

        public async Task<(bool isClean, string reason)> AnalyzeAsync(string text)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post,
                    "https://api.anthropic.com/v1/messages");

                request.Headers.Add("x-api-key", _apiKey);
                request.Headers.Add("anthropic-version", "2023-06-01");

                var prompt = $"""
                    Aşağıdaki yorum metnini analiz et. 
                    Metin hakaret, küfür, ırkçılık, nefret söylemi, şiddet veya uygunsuz içerik barındırıyor mu?
                    
                    Yorum: "{text}"
                    
                    Sadece şu formatta yanıt ver (başka hiçbir şey yazma):
                    TEMIZ veya UYGUNSUZ: [kısa açıklama]
                    """;

                var body = new
                {
                    model = "claude-haiku-4-5-20251001",
                    max_tokens = 100,
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    }
                };

                request.Content = new StringContent(
                    JsonSerializer.Serialize(body),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseBody);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMsg = doc.RootElement
                        .TryGetProperty("error", out var err)
                        ? err.GetProperty("message").GetString()
                        : "Bilinmeyen hata";
                    return (true, $"API Hatası: {errorMsg}");
                }

                var result = doc.RootElement
                    .GetProperty("content")[0]
                    .GetProperty("text")
                    .GetString() ?? "";

                if (result.TrimStart().StartsWith("TEMIZ", StringComparison.OrdinalIgnoreCase))
                    return (true, "Onaylandı");

                if (result.TrimStart().StartsWith("UYGUNSUZ", StringComparison.OrdinalIgnoreCase))
                {
                    var reason = result.Contains(":")
                        ? result.Substring(result.IndexOf(':') + 1).Trim()
                        : "Uygunsuz içerik tespit edildi.";
                    return (false, reason);
                }

                // Belirsiz yanıt → onayla
                return (true, "Onaylandı");
            }
            catch (Exception ex)
            {
                return (true, $"Moderasyon hatası: {ex.Message}");
            }
        }
    }
}