using System.Net.Http.Headers;
using System.Text.Json;

namespace GeekShopping.Web.Utils
{
    public static class HttpClientExtentions
    {
        private static readonly MediaTypeHeaderValue _contetTypeHeader = new MediaTypeHeaderValue("application/json");
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Request failed with status code: {response.StatusCode}, and reason {response.ReasonPhrase}");
            }
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<T>(content, JsonOptions);
        }

        public static Task<HttpResponseMessage> PostAsJson<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = _contetTypeHeader;
            return httpClient.PostAsync(url, content);
        }
        public static Task<HttpResponseMessage> PutAsJson<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = _contetTypeHeader;
            return httpClient.PutAsync(url, content);
        }
    }
}
