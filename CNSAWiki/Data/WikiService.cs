using CNSAWiki.Data;
using CNSAWiki.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace CNSAWiki.Data
{
    public class WikiService
    {
        private readonly HttpClient _http;
        private readonly AuthService _auth;

        public WikiService(HttpClient http, AuthService auth)
        {
            _http = http;
            _auth = auth;
        }

        private void AddToken()
        {
            var token = _auth.JwtToken; // AuthService에서 JWT 가져오기
            if (!string.IsNullOrEmpty(token))
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public Task CreatePageAsync(WikiPage page)
        {
            AddToken();
            return _http.PostAsJsonAsync("api/wiki", page);
        }

        public Task UpdatePageAsync(WikiPage page)
        {
            AddToken();
            return _http.PutAsJsonAsync($"api/wiki/{page.PageId}", page);
        }

        public Task DeletePageAsync(long id)
        {
            AddToken();
            return _http.DeleteAsync($"api/wiki/{id}");
        }

        // 조회(GET) 요청은 토큰 필요 없음
        public Task<List<WikiPage>> GetPagesAsync()
            => _http.GetFromJsonAsync<List<WikiPage>>("api/wiki") ?? Task.FromResult(new List<WikiPage>());

        public Task<WikiPage?> GetPageAsync(long id)
            => _http.GetFromJsonAsync<WikiPage>($"api/wiki/{id}");

        public async Task<string?> UploadImageAsync(IBrowserFile file)
        {
            AddToken(); // 업로드도 인증 필요하면 사용

            using var content = new MultipartFormDataContent();
            var stream = file.OpenReadStream(maxAllowedSize: 20 * 1024 * 1024); // 20MB 제한

            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            content.Add(fileContent, "file", file.Name);

            var response = await _http.PostAsync("api/wiki/upload", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<UploadResult>();
            return result?.Url;
        }

        public class UploadResult
        {
            public string Url { get; set; } = "";
        }

    }
}
