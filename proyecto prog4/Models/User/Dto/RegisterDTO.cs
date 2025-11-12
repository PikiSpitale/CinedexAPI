using System.ComponentModel.DataAnnotations;

namespace proyecto_prog4.Models.User.Dto
{
    public class RegisterDTO
    {
        [Required]
        [MinLength(4)]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = null!;
        
        [Required]
        [MinLength(8)]
        public string ConfirmPassword { get; set; } = null!;
    }
}