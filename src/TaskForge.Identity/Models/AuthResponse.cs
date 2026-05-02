using System;
using System.Collections.Generic;
using System.Text;

namespace TaskForge.Identity.Models
{
    public class AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public int ExpiresIn { get; set; }

        public string Email { get; set; } = string.Empty;

        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
