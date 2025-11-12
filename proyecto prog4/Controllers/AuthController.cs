using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyecto_prog4.Enums;
using proyecto_prog4.Models.User.Dto;
using proyecto_prog4.Services;
using proyecto_prog4.Utils;

namespace proyecto_prog4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthServices _authServices;

        public AuthController(AuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid credentials" });

            try
            {
                var result = await _authServices.LoginAsync(dto, HttpContext);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input" });

            try
            {
                var result = await _authServices.RegisterAsync(dto);
                return CreatedAtAction(nameof(Register), new { token = result.Token }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        async public Task<ActionResult> Logout()
        {
            try
            {
                await _authServices.Logout(HttpContext);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new HttpMessage(ex.Message)
                );
            }
        }


        [HttpPut("{id}/roles")]
        [Authorize(Roles = ROL.ADMIN)]
        async public Task<ActionResult<UsuarioWithRolesDTO>> AssignRoles(int id, [FromBody] List<int> rolesIds)
        {
            try
            {
                return Ok(await _authServices.AssignRoles(id, rolesIds));
            }
            catch (HttpResponseError ex)
            {
                return StatusCode(
                    (int)ex.StatusCode,
                    new HttpMessage(ex.Message)
                );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new HttpMessage(ex.Message)
                );
            }
        }

        [HttpGet("health")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<bool> Health()
        {
            try
            {
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new HttpMessage(ex.Message)
                );
            }
        }
    }
}