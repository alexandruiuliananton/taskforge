using Operis.Identity.Users;

namespace Operis.Identity.Jwt
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser applicationUser);  
    }
}
