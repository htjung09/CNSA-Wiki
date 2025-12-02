using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.JSInterop;
using System;

namespace CNSAWiki.Data
{
    public class AuthService
    {
        private readonly HttpClient _http;

        public bool Authorized { get; private set; } = false;
        public string Username { get; private set; } = string.Empty;
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
            
            Username = username ?? "";

            SetToken(result.token);

            return true;
        }

        public class LoginResult
        {
            public string token { get; set; } = "";
            public string? username { get; set; }
        }

        public class MyInfoResponse
        {
            public string? username { get; set; }
        }
    }
}
