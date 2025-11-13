using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyecto_prog4.Enums;
using proyecto_prog4.Models.Rol.Dto;
using proyecto_prog4.Services;
using System.Collections.Generic;
using System.Linq;

namespace proyecto_prog4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = ROL.ADMIN)]
    public class RolController : ControllerBase
    {
        private readonly RolServices _rolServices;

        public RolController(RolServices rolServices)
        {
            _rolServices = rolServices;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<RolDTO>>> GetAll()
        {
            var roles = await _rolServices.GetAll();
            var dto = roles
                .Select(r => new RolDTO { Id = r.Id, Nombre = r.Nombre })
                .ToList();
            return Ok(dto);
        }
    }
}