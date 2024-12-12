using System.Security.Claims;
using System.Security.Principal;

namespace Microsoft.IdentityModel.Tokens
{
    public interface ITokenProvider
    {
        TokenInfo Create(string userid, string username, params Claim[] claims);
        IPrincipal Validate(string token);
    }
}
