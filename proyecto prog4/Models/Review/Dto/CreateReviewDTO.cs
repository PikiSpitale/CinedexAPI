using System.ComponentModel.DataAnnotations;

namespace proyecto_prog4.Models.Review.Dto
{
    public class CreateReviewDTO
    {
        [Required]
        public string Content { get; set; } = null!;

        [Range(1, 10)]
        public int Rating { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
