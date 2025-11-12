using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyecto_prog4.Models.Rol
{
    public class Rol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }

    public class RolUsuario
    {
        public int RolId { get; set; }
        public int UsuarioId { get; set; }
    }
}
