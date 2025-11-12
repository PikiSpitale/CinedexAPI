using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyecto_prog4.Models.Review.Dto;
using proyecto_prog4.Services;
using System.Security.Claims;

namespace proyecto_prog4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewServices _reviewService;

        public ReviewController(ReviewServices reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _reviewService.GetAll();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await _reviewService.GetById(id);
            if (review == null)
                return NotFound(new { message = "Review not found" });

            return Ok(review);
        }

        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetByMovieId(int movieId)
        {
            var reviews = await _reviewService.GetByMovieId(movieId);
            return Ok(reviews);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var reviews = await _reviewService.GetByUserId(userId);
            return Ok(reviews);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateReviewDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var review = await _reviewService.Create(dto);
                return CreatedAtAction(nameof(GetById), new { id = review.Id }, review);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _reviewService.Update(id, dto);
            if (!result)
                return NotFound(new { message = "Review not found" });

            return Ok(new { message = "Review updated successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _reviewService.Delete(id);
            if (!result)
                return NotFound(new { message = "Review not found" });

            return NoContent();
        }
    }
}