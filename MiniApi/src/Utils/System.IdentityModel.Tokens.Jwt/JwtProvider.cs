using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;

namespace System.IdentityModel.Tokens.Jwt
{
    public sealed class JwtProvider : ITokenProvider
    {
        private readonly SigningCredentials _credentials;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly TimeSpan _duration;
        private readonly JwtSecurityTokenHandler _handler;

        public JwtProvider(byte[] key, string algorithm = null, string issuer = null, string audience = null, string duration = null)
        {
            _credentials = new SigningCredentials(new SymmetricSecurityKey(key), algorithm ?? SecurityAlgorithms.HmacSha256);
            _issuer = issuer ?? Assembly.GetExecutingAssembly().GetName().Name;
            _audience = audience ?? $"{_issuer}.User";
            _duration = TimeSpan.Parse(duration ?? "08:00");
            _handler = new JwtSecurityTokenHandler();
        }

        public TokenInfo Create(string userid, string username, params Claim[] claims)
        {
            DateTime now = DateTime.Now;
            DateTime notBefore = now.AddMinutes(-30);
            DateTime expires = now.Add(_duration);
            Claim jti = new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"));
            Claim iat = new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(now.ToUniversalTime()).ToString());
            Claim sub = new Claim(JwtRegisteredClaimNames.Sub, userid);
            Claim name = new Claim(JwtRegisteredClaimNames.UniqueName, username);
            JwtSecurityToken token = new JwtSecurityToken(_issuer, _audience, claims.Concat(new[] { jti, iat, sub, name }), notBefore, expires, _credentials);
            return new TokenInfo(token, _handler);
        }

        public IPrincipal Validate(string token)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _credentials.Key,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
            };
            return _handler.ValidateToken(token, validationParameters, out _);
        }
    }
}
