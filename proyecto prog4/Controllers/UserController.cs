using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyecto_prog4.Enums;
using proyecto_prog4.Models.User.Dto;
using proyecto_prog4.Services;
using proyecto_prog4.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace proyecto_prog4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = ROL.ADMIN)]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userServices;
        private readonly IMapper _mapper;

        public UserController(UserServices userServices, IMapper mapper)
        {
            _userServices = userServices;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UsuarioWithRolesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<UsuarioWithRolesDTO>>> GetAll()
        {
            var users = await _userServices.GetAllUsersAsync();
            var dto = _mapper.Map<List<UsuarioWithRolesDTO>>(users);
            return Ok(dto);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(UsuarioWithRolesDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UsuarioWithRolesDTO>> GetOne(int id)
        {
            try
            {
                var user = await _userServices.GetOneById(id);
                return Ok(_mapper.Map<UsuarioWithRolesDTO>(user));
            }
            catch (HttpResponseError ex)
            {
                return StatusCode((int)ex.StatusCode, new HttpMessage(ex.Message));
            }
        }
    }
}