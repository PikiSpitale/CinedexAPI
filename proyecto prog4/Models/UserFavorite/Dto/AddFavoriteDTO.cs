using System.ComponentModel.DataAnnotations;

namespace proyecto_prog4.Models.UserFavorite.Dto
{
    public class AddFavoriteDTO
    {
        [Required]
        public int MovieId { get; set; }
    }
}
