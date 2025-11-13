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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpMessage))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(HttpMessage))]
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegisterDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpMessage))]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(HttpMessage))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(HttpMessage))]
        public async Task<ActionResult> Logout()
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioWithRolesDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpMessage))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(HttpMessage))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(HttpMessage))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(HttpMessage))]
        public async Task<ActionResult<UsuarioWithRolesDTO>> AssignRoles(int id, [FromBody] List<int> rolesIds)
        {
            try
            {
                return Ok(await _authServices.AssignRoles(id, rolesIds));
            }
            catch (HttpResponseError ex)
            {
                return StatusCode((int)ex.StatusCode, new HttpMessage(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new HttpMessage(ex.Message));
            }
        }
    }
}