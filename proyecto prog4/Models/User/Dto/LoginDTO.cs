using System.ComponentModel.DataAnnotations;

namespace proyecto_prog4.Models.User.Dto
{
    public class LoginDTO
    {
        [Required]
        public string EmailOrUsername { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}