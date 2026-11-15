using System.ComponentModel.DataAnnotations;

namespace Operis.Api.Dto
{
    public class RegisterRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
