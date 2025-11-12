using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyecto_prog4.Models.Usuario
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
        public ICollection<UserFavorite.UserFavorite> Favorites { get; set; } = new List<UserFavorite.UserFavorite>();
        public List<Rol.Rol> Roles { get; set; } = new();
    }
}
