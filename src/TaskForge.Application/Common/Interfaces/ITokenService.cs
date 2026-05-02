using TaskForge.Identity.Users;

namespace TaskForge.Identity.Jwt
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser applicationUser);  
    }
}
