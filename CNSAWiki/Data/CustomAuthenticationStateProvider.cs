using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CNSAWiki.Data
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly AuthService _authService;

        public CustomAuthStateProvider(AuthService authService)
        {
            _authService = authService;
            _authService.OnChange += NotifyAuthChanged;
        }

        private void NotifyAuthChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity;
            if (_authService.Authorized)
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, _authService.Username)
                }, "jwt");
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            var user = new ClaimsPrincipal(identity);
            return Task.FromResult(new AuthenticationState(user));
        }
    }
}
