using CNSAWiki.Data;
using CNSAWiki.Models;


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
    }
}
