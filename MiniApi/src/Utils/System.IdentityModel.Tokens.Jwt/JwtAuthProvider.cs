using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;

namespace System.IdentityModel.Tokens.Jwt
{
    public sealed class JwtAuthProvider : IAuthProvider
    {
        private readonly SigningCredentials _credentials;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public string Issuer { get; }
        public string Audience { get; }
        public TimeSpan Duration { get; }

        public JwtAuthProvider(byte[] key, string algorithm = null, string issuer = null, string audience = null, string duration = null)
        {
            _credentials = new SigningCredentials(new SymmetricSecurityKey(key), algorithm ?? SecurityAlgorithms.HmacSha256);
            _tokenHandler = new JwtSecurityTokenHandler();
            Issuer = issuer ?? AppDomain.CurrentDomain.FriendlyName;
            Audience = audience ?? $"{AppDomain.CurrentDomain.FriendlyName}.Client";
            Duration = TimeSpan.Parse(duration ?? "08:00");
        }

        public string Authorize(params Claim[] claims)
        {
            DateTime now = DateTime.Now;
            DateTime notBefore = now.AddMinutes(-30);
            DateTime expires = now.Add(Duration);
            JwtSecurityToken token = new JwtSecurityToken(Issuer, Audience, claims, notBefore, expires, _credentials);
            return _tokenHandler.WriteToken(token);
        }

        public IPrincipal Authenticate(string token)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _credentials.Key,
                ValidIssuer = Issuer,
                ValidAudience = Audience,
            };
            return _tokenHandler.ValidateToken(token, validationParameters, out _);
        }
    }
}
