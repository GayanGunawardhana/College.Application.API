using System.ComponentModel.DataAnnotations;

namespace College.App.Models
{
    public class LoginRequestDTO
    {   
        [Required]
        public string Policy { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
