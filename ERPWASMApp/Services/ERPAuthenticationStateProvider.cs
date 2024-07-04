using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ERPWASMApp.Services{
    public class ERPAuthenticationStateProvider : AuthenticationStateProvider{
        public override async Task<AuthenticationState> GetAuthenticationStateAsync(){
            return await Task.FromResult(new AuthenticationState(AnonymousUser));
        }

        private ClaimsPrincipal AnonymousUser => new(new ClaimsIdentity(Array.Empty<Claim>()));
        private ClaimsPrincipal TestUser{
            get {
                var claims = new[] {
                    new Claim(ClaimTypes.Name, "UserPrueba1"),
                    new Claim(ClaimTypes.Role, "User")
                };
                var identity = new ClaimsIdentity(claims, "test");
                return new ClaimsPrincipal(identity);
            }
        }

        private ClaimsPrincipal TestAdmin { 
            get {
                var claims = new[]{
                    new Claim(ClaimTypes.Name, "AdminPrueba"),
                    new Claim(ClaimTypes.Role, "Admin")
                };
                var identity = new ClaimsIdentity(claims, "test");
                return new ClaimsPrincipal(identity);
            } 
        }

        public void TestSignIn() {
            var result = Task.FromResult(new AuthenticationState(TestUser));
            NotifyAuthenticationStateChanged(result);
        }

        public void TestAdminSignIn() {
            var result = Task.FromResult(new AuthenticationState(TestAdmin));
            NotifyAuthenticationStateChanged(result);
        }

        public void TestSignOut() {
            var result = Task.FromResult(new AuthenticationState(AnonymousUser));
            NotifyAuthenticationStateChanged(result);
        }
    }
}
