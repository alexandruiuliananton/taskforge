using Microsoft.AspNetCore.Identity;

namespace Operis.Identity.Users
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
    }
}
