using System;

namespace Microsoft.IdentityModel.Tokens
{
    public class TokenInfo
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public TokenInfo(SecurityToken token, SecurityTokenHandler handler)
        {
            Token = handler.WriteToken(token);
            Expiration = token.ValidTo.ToLocalTime();
        }
    }
}
