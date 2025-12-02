using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.JSInterop;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SharedData.DTOs;

namespace CNSAWiki.Data
{
    public class AuthService
    {
        private readonly HttpClient _http;

        public bool Authorized { get; private set; } = false;
        public string Username { get; private set; } = string.Empty;
        public long UserId { get; private set; }
        public string JwtToken { get; private set; } = string.Empty;

        public event Action? OnChange;
        private void Notify() => OnChange?.Invoke();

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public void SetToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                JwtToken = "";
                Authorized = false;
                _http.DefaultRequestHeaders.Authorization = null;
                Notify();
                return;
            }

            JwtToken = token;
            Authorized = true;
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            Notify();
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", new { username, password });
            if (!response.IsSuccessStatusCode) return false;

            var result = await response.Content.ReadFromJsonAsync<LoginResult>();
            if (result == null || string.IsNullOrEmpty(result.token)) return false;
            
            SetToken(result.token);
            DecodeToken(result.token);

            return true;
        }

        // ⭐ 회원가입
        public async Task<(bool success, string message)> RegisterAsync(string username, string password)
        {
            var dto = new RegisterDto
            {
                Username = username,
                Password = password
            };

            var response = await _http.PostAsJsonAsync("api/auth/register", dto);

            if (response.IsSuccessStatusCode)
            {
                var m = await response.Content.ReadAsStringAsync();
                return (true, m);
            }
            else
            {
                var err = await response.Content.ReadAsStringAsync();
                return (false, err);
            }
        }

        private void DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == "userId");
            var userNameClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out var id))
                UserId = id;
            if (userNameClaim != null)
                Username = userNameClaim.Value;
        }

        public class LoginResult
        {
            public string token { get; set; } = "";
            public string? username { get; set; }
        }
    }
}
