using Microsoft.AspNetCore.Identity;

namespace TaskForge.Identity.Users
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
    }
}
